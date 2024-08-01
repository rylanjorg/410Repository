using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;

//[RequireComponent(typeof(VFXSpawner), typeof(VFXEventController))]
public class ProceduralLegController : MonoBehaviour
{
    public enum State
    {
        Moving,
        Idle,
    }
  

    [Title("Ground Intersection:")]
    [TabGroup("tab1", "General")] [SerializeField] private LayerMask groundLayer; // LayerMask to filter the ground layer
    [TabGroup("tab1", "General")] [SerializeField] Vector3 rayCastYOffset = new Vector3(0, 1f, 0);
    [TabGroup("tab1", "General")] [SerializeField] Transform root;
    [TabGroup("tab1", "General")] [SerializeField] float minimumSpeedThreshold = 0.1f;

    [TabGroup("tab1", "General")] [SerializeField] public List<ProceduralLeg> legs = new List<ProceduralLeg>();
    Queue<ProceduralLeg> rightLegQueue = new Queue<ProceduralLeg>();
    Queue<ProceduralLeg> leftLegQueue = new Queue<ProceduralLeg>();
    

    [TabGroup("tab1", "Debug")] [SerializeField] bool drawMidPoint = false;
    [TabGroup("tab1", "Debug")] [SerializeField] bool drawTarget = false;
  

    [Title("Leg Rotation Settings:")]
    [TabGroup("tab1", "Second Order Dynamics", TextColor = "orange")] [SerializeField] UpdateMode updateMode;
    [TabGroup("tab1", "Second Order Dynamics")] [SerializeField] float updateInterval = 0.001f;
    [TabGroup("tab1", "Second Order Dynamics")] [SerializeField] float f = 1.0f;
    [TabGroup("tab1", "Second Order Dynamics")] [SerializeField] float z = 1.0f;
    [TabGroup("tab1", "Second Order Dynamics")] [SerializeField] float r = 1.0f;
    [TabGroup("tab1", "Second Order Dynamics")] [SerializeField] bool scaleFWithSpeed = true;
       
    [ShowIfGroup("tab1/Second Order Dynamics/scaleFWithSpeed")]
    [SerializeField] float rateOfGrowthFactor_speed = 0.3f;
    [TabGroup("tab1", "Second Order Dynamics")] [SerializeField] bool scaleFWithDistance = true;
    [ShowIfGroup("tab1/Second Order Dynamics/scaleFWithDistance")]
    [SerializeField] float rateOfGrowthFactor_distance = 0.3f;

   

    [TabGroup("tab1", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] float currentSpeed;
    [TabGroup("tab1", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] Vector3 prevPos;
    [TabGroup("tab1", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] float distanceMultipler;
    [TabGroup("tab1", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] float speedMultipler;
    [TabGroup("tab1", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] public State currentState;

    int lastEnqueuedLegIndex = 2; // Initialize to 2 because you enqueued legs 0 and 2 in Start()



    void Start()
    {
        OnStart(() => {
            for(int i = 0; i < legs.Count; i++)
            {
                legs[i].easyIK.OnAwake();
            }
        });
        

        // Enqueue legs 1 and 2
        leftLegQueue.Enqueue(legs[0]);
        leftLegQueue.Enqueue(legs[1]);

        rightLegQueue.Enqueue(legs[2]);
        rightLegQueue.Enqueue(legs[3]);

        // Enqueue legs 2 and 4
        //legQueue.Enqueue(legs[1]);
        //legQueue.Enqueue(legs[3]);
        prevPos = root.position;
    }

    void OnStart(Action callback)
    {
        for(int i = 0; i < legs.Count; i++)
        {
            // Instantiate Instance of the Target
            GameObject targetInstance = Instantiate(legs[i].targetPrefab, legs[i].targetPrefab.transform.position, Quaternion.identity);
            
            ProceduralLeg leg = legs[i];

            // Set References
            leg.tempTargetTransform = targetInstance.transform;
            leg.easyIK.ikTarget = targetInstance.transform;
            legs[i] = leg;

            // Set Second Order Dynamcis
            legs[i].secondOrderDynamics = new SecondOrderDynamics(f, z, r, legs[i].CastTempToLocalSpace());
            legs[i].vfxSpawner = GetComponent<VFXSpawner>();

        }

        // Call the callback function after all targets have been created
        callback?.Invoke();
    }

    void UpdateSystemState()
    {
        bool isIdle = false;
        int movingCount = 0;
        // Iterate over all legs
        for(int i = 0; i < legs.Count; i++)
        {
            // If any leg is not in the idle state, set isIdle to false and break the loop
            if (legs[i].targetState != ProceduralLeg.TargetState.Idle)
            {
                movingCount++;
            }
        }

        if(currentSpeed < 0.4f && movingCount <= 2)
        {
            isIdle = true;
        }

        if(isIdle)
        {
            currentState = State.Idle;
        }
        else
        {
            currentState = State.Moving;
        }
    }


    void Update()
    {   
        keepTargetsGrounded();
        
        /// Summary:
        /// So the idea is that i dequeue the leg, move it, trigger some callback when it's grounded again to make the next leg go, then requeue it.
        /// At that point it has to wait for the callback of the sent out leg to go again, and that cycles the legs. 
       
        currentSpeed = Vector3.Distance(root.position, prevPos) / Time.deltaTime;
        
        
        // If there are legs in the queue and the currently moving leg has finished moving
        if (leftLegQueue.Count > 0 && !IsLegMoving(legs[0]) && !IsLegMoving(legs[1]))
        {
            bool distanceCheckLeft = Vector3.Distance(leftLegQueue.Peek().tempTargetTransform.position, leftLegQueue.Peek().targetTransform.position) > leftLegQueue.Peek().stepDistanceThreshold;
            Debug.DrawRay(leftLegQueue.Peek().tempTargetTransform.position, leftLegQueue.Peek().targetTransform.position - leftLegQueue.Peek().tempTargetTransform.position, distanceCheckLeft? Color.green : Color.red);

            if(distanceCheckLeft && currentSpeed > minimumSpeedThreshold)
            {
                // Dequeue the next leg and start moving it
                ProceduralLeg nextLeg = leftLegQueue.Dequeue();  
                
                StartCoroutine(MoveLeg(nextLeg, () => 
                {
                    // Requeue the leg
                    leftLegQueue.Enqueue(nextLeg);
                    Debug.Log("left leg callback");
                    OnLegCompleteAction(nextLeg);
                }, () => 
                {
                    // Requeue the leg
                    leftLegQueue.Enqueue(nextLeg);
                    Debug.Log("left leg callback");
                    OnLegCompleteAction(nextLeg);
                }
                ));
            }
        }

        

        // If there are legs in the queue and the currently moving leg has finished moving
        if (rightLegQueue.Count > 0 && !IsLegMoving(legs[2]) && !IsLegMoving(legs[3]))
        {
            bool distanceCheckRight = Vector3.Distance(rightLegQueue.Peek().tempTargetTransform.position, rightLegQueue.Peek().targetTransform.position) > rightLegQueue.Peek().stepDistanceThreshold;
            Debug.DrawRay(rightLegQueue.Peek().tempTargetTransform.position, rightLegQueue.Peek().targetTransform.position - rightLegQueue.Peek().tempTargetTransform.position, distanceCheckRight ? Color.green : Color.red);

            if(distanceCheckRight && currentSpeed > minimumSpeedThreshold)
            {   
                // Dequeue the next leg and start moving it
                ProceduralLeg nextLeg = rightLegQueue.Dequeue();  
                
                StartCoroutine(MoveLeg(nextLeg, () => 
                {
   
                    // Requeue the leg
                    rightLegQueue.Enqueue(nextLeg);
                    Debug.Log("right leg callback");
                    OnLegCompleteAction(nextLeg);
               
                }, () => 
                {
                    // Requeue the leg
                    rightLegQueue.Enqueue(nextLeg);
                    Debug.Log("right leg callback");
                    OnLegCompleteAction(nextLeg);
                }
                ));
            }
        }

        for(int i = 0; i < legs.Count; i++)
        {
            legs[i].easyIK.SolveIK();
        }

        UpdateSystemState();
        prevPos = root.position;
    }

    void EnqueueNextLeftLeg(ProceduralLeg nextLeg)
    {
        leftLegQueue.Enqueue(nextLeg);
        Debug.Log("Enqueued left leg " + nextLeg.legIndex);
    }

    void EnqueueNextRightLeg(ProceduralLeg nextLeg)
    {
        rightLegQueue.Enqueue(nextLeg);
        Debug.Log("Enqueued right leg " + nextLeg.legIndex);
    }


    void keepTargetsGrounded()
    {
        

        for(int i = 0; i < legs.Count; i++)
        {
            Vector3 footJointDifference = legs[i].lastJointTransform.position - legs[i].footTransform.position;
            RaycastHit hit;

            if (Physics.Raycast(legs[i].targetTransform.position + rayCastYOffset, -Vector3.up, out hit, Mathf.Infinity, groundLayer))
            {
                // If the raycast hits something, set the target transform's Y position to the hit point's Y position
                Vector3 targetPosition = legs[i].targetTransform.position;
                targetPosition.y = hit.point.y;
                legs[i].targetTransform.position = targetPosition;
            }

            if(legs[i].targetState != ProceduralLeg.TargetState.Idle) continue;
            
            RaycastHit hitTemp;

            if (Physics.Raycast(legs[i].tempTargetTransform.position + rayCastYOffset, -Vector3.up, out hitTemp, Mathf.Infinity, groundLayer))
            {
                // If the raycast hits something, set the target transform's Y position to the hit point's Y position
                Vector3 targetPosition = legs[i].tempTargetTransform.position;
                targetPosition.y = hitTemp.point.y + GetFootJointDifferenceWorld(i).y;
                legs[i].tempTargetTransform.position = targetPosition;
                
            }

    
       
            legs[i].IsFootGrounded(groundLayer);
            
       }
    }

 

    bool IsLegMoving(ProceduralLeg leg)
    {
        // Implement this method to return whether the given leg is currently moving
        // This could be based on the state of the EasyIK component, or another indicator of movement
        return leg.isMoving;
    }

    void OnLegCompleteAction(ProceduralLeg leg)
    {
        
        //Debug.Log("on leg complete action");
        leg.isMoving = false;
        leg.reachedMidPoint = false;
        leg.targetState = ProceduralLeg.TargetState.Idle;
        legs[leg.legIndex] = leg;
    }

    Vector3 GetFootJointDifference(int index)
    {
        // Convert the last joint's position to local space
        Vector3 lastJointPosition_LS = legs[index].baseTransform.InverseTransformPoint(legs[index].lastJointTransform.position);

        // Convert the foot's position to local space
        Vector3 footPosition_LS = legs[index].baseTransform.InverseTransformPoint(legs[index].footTransform.position);

        // Calculate the difference between the last joint and the foot
        Vector3 footJointDifference = lastJointPosition_LS - footPosition_LS;

        footJointDifference.x = 0;
        footJointDifference.z = 0;

        return footJointDifference;
    }

    Vector3 GetFootJointDifferenceWorld(int index)
    {
       
        // Calculate the difference between the last joint and the foot
        Vector3 footJointDifference = legs[index].lastJointTransform.position - legs[index].footTransform.position;

        footJointDifference.x = 0;
        footJointDifference.z = 0;

        return footJointDifference;
    }



    IEnumerator MoveLeg(ProceduralLeg leg, Action queueNextLegCallback, Action onComplete)
    {
        int index = leg.legIndex;

        float elapsedTime = 0f;
        Vector3 FootJointDifference = GetFootJointDifference(index);
        Vector3 prevTargetPosition = legs[index].targetTransform.position;
        legs[index].secondOrderDynamics.UpdateStartPosition(legs[index].CastTempToLocalSpace());


        legs[index].OnMoveLeg();
        while (elapsedTime < legs[index].stepDuration || legs[index].isFootGrounded == false)
        {
          
            // Update the Second Order Dynamics Constants
            UpdateSecondOrderConstants(legs[index]);

            if(elapsedTime >= leg.nextLegCallbackTime)
            {   
                legs[index].tempTargetTransform.position = legs[index].targetTransform.position;
                break;
            }

            // Update the midpoint and check if it has been reached
            legs[index].UpdateMidPoint(currentSpeed);
            legs[index].UpdateReachedMidpoint(elapsedTime);

            // Calculate adjusted speed based on distance
            float adjustedSpeed = Mathf.Clamp(legs[index].distanceToTarget, 1.0f, Mathf.Infinity);
            

            Vector3 targetPosition;
            // Move towards midpoint if not reached yet, else move towards target
            switch(legs[index].targetState)
            {
                case ProceduralLeg.TargetState.MidPoint:
                    targetPosition = legs[index].midPoint;
                    Debug.DrawRay(root.TransformPoint(targetPosition),Vector3.up, Color.yellow);
                    break;
                case ProceduralLeg.TargetState.EndTarget:
                    targetPosition = legs[index].targetTransform.localPosition;
                    Debug.DrawRay(root.TransformPoint(targetPosition),Vector3.up, Color.green);
                    break;
                default:
                    targetPosition = legs[index].targetTransform.localPosition;
                    break;
            }
           

            // Convert the target position to local space
            legs[index].currentTargetPosition = targetPosition;

            // Update the leg position in local space
            legs[index].currentSODInputPosition = legs[index].CastTempToLocalSpace();
            legs[index].SODynamicsOutputPosition = legs[index].secondOrderDynamics.Update(Time.deltaTime * adjustedSpeed, targetPosition, legs[index].currentSODInputPosition);

            legs[index].SODynamicsOutputPosition += FootJointDifference;
            // Convert the foot's position to local space
            //Vector3 footPosition_LS = legs[index].baseTransform.InverseTransformPoint(legs[index].footTransform.position);

            // Calculate the difference between the foot joint and the target position
            //Vector3 footJointDifference = footPosition_LS - legs[index].SODynamicsOutputPosition;

            // Adjust the target position by the foot length
            //legs[index].SODynamicsOutputPosition += footJointDifference;
                        
            Debug.Log("foot joint difference: " + FootJointDifference);

            // Convert the updated position back to world space
            Vector3 Pos_WS = legs[index].baseTransform.TransformPoint(legs[index].SODynamicsOutputPosition);

            // Adjust the target position by the foot length
            legs[index].tempTargetTransform.position = Pos_WS;

            // Update elapsed time and distance
            elapsedTime += Time.deltaTime;
            prevTargetPosition = legs[index].targetTransform.position;
            
            // Calculate the distance between temp and target
            legs[index].UpdateDistance();
            legs[index].IsFootGrounded(groundLayer);
            

            yield return new WaitForSeconds(updateInterval);
        }

        //legs[index].IsFootGrounded(groundLayer);
        // Update the legs list with the modified leg struct one last time before calling onComplete
        legs[leg.legIndex] = leg;

        onComplete?.Invoke();
    }

    void UpdateSecondOrderConstants(ProceduralLeg leg)
    {
        if(scaleFWithSpeed || scaleFWithDistance)
        {

            speedMultipler = scaleFWithSpeed ? Mathf.Pow((1 + rateOfGrowthFactor_speed), currentSpeed) : 1;

            distanceMultipler = scaleFWithDistance ? Mathf.Pow((1 + rateOfGrowthFactor_distance), leg.distanceToTarget) : 1;

            leg.secondOrderDynamics.SetConstants(f * (speedMultipler * distanceMultipler), z, r);
        }
    }

    void OnDrawGizmos()
    {
        if(drawMidPoint)
        {
            for(int i = 0; i < legs.Count; i++)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(legs[i].baseTransform.TransformPoint(legs[i].midPoint), 0.2f);
                //Gizmos.color = Color.red;
                //Gizmos.DrawRay(legs[i].tempTargetTransform.position, Vector3.up * 10);
            }
        }

        if(drawTarget)
        {
            for(int i = 0; i < legs.Count; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(legs[i].baseTransform.TransformPoint(legs[i].targetTransform.localPosition), 0.2f);
                
            }
        }
    }
}
