using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;

public enum PAState
{
    Moving,
    Idle
}

[RequireComponent(typeof(SecondOrderDemo))]
public class ProceduralAnimTargetController : MonoBehaviour
{
    /*
    [TabGroup("tab1", "General", TextColor = "green")] [SerializeField] private List<Transform> footTransforms = new List<Transform>();
    [TabGroup("tab1", "General")] [SerializeField] private List<Transform> targetTransforms = new List<Transform>();
    [TabGroup("tab1", "General")] [SerializeField] private List<GameObject> targetPrefab = new List<GameObject>();
    [TabGroup("tab1", "General")] [SerializeField] private List<EasyIK> easyIK = new List<EasyIK>();
    [TabGroup("tab1", "General")] [SerializeField] private ProceduralLegController proceduralLegController;

    

    [Title("Step Settings:")]
    [TabGroup("tab1", "General")] [SerializeField] bool scaleStepDistanceWithSpeed = true;
    [ShowIfGroup("tab1/General/scaleStepDistanceWithSpeed")] [SerializeField] float rateOfGrowthFactor_stepDistance = 0.3f;
    
    [TabGroup("tab1", "General")] [SerializeField] float stepDistanceThreshold = 1.0f;
    [TabGroup("tab1", "General")] [SerializeField] float individualStepDuration = 0.5f;
    [TabGroup("tab1", "General")] [SerializeField] [PropertyRange(0.01, "individualStepDuration")] float timeBetweenSteps = 0.4f;
    [TabGroup("tab1", "General")] [SerializeField] float speedThreshold = 0.1f;

    [Title("Step Arc Settings:")]
    [TabGroup("tab1", "General")] [SerializeField] [Range(0.1f,0.95f)] float reachMidPointTimeThreshold = 0.3f;
    [TabGroup("tab1", "General")] [SerializeField]  float midPointDistanceThreshold = 0.6f;
    [TabGroup("tab1", "General")] [SerializeField] float midPointHeight = 0.5f;

    

   
    [Title("Leg Rotation Settings:")]
    [TabGroup("tab1", "Second Order Dynamics", TextColor = "orange")] [SerializeField] UpdateMode updateMode;
    [TabGroup("tab1", "Second Order Dynamics")] [SerializeField] float updateInterval = 0.1f;
    [TabGroup("tab1", "Second Order Dynamics")] [SerializeField] float f = 1.0f;
    [TabGroup("tab1", "Second Order Dynamics")] [SerializeField] float z = 1.0f;
    [TabGroup("tab1", "Second Order Dynamics")] [SerializeField] float r = 1.0f;
    [TabGroup("tab1", "Second Order Dynamics")] [SerializeField] bool scaleFWithSpeed = true;
       
    [ShowIfGroup("tab1/Second Order Dynamics/scaleFWithSpeed")]
    [SerializeField] float rateOfGrowthFactor_speed = 0.3f;
    [TabGroup("tab1", "Second Order Dynamics")] [SerializeField] bool scaleFWithDistance = true;
    [ShowIfGroup("tab1/Second Order Dynamics/scaleFWithDistance")]
    [SerializeField] float rateOfGrowthFactor_distance = 0.3f;

    
    

 




    [TabGroup("tab1", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] private List<Transform> tempTransforms = new List<Transform>();
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] [ListDrawerSettings(ShowFoldout = true, DraggableItems = false, ShowItemCount = false)] bool[] hasMoved = new bool[4];
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] [ListDrawerSettings(ShowFoldout = true, DraggableItems = false, ShowItemCount = false)] bool[] currentlyUpdating = new bool[4];
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] bool onEnter = false;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] bool update14 = false;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] bool update23 = false;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] int currentStep = 0;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] int coroutineIndex = 0;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] private int tempNum = 0;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] PAState currentState;
    
    public PAState CurrentState 
    { 
        get { return currentState; } 
        private set { currentState = value; } 
    }

    [Title("Custom Properties:")]
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] float stepDistanceScalar = 1.0f;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] float distanceMultipler = 1.0f;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] float speedMultipler = 1.0f;
    
    [Title("Current Speed:")]
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] float currentSpeed;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] Vector3 previousPosition;
   
    [Title("SecondOrderDynamics:")]
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] List<SecondOrderDynamics> secondOrderDynamics = new List<SecondOrderDynamics>();

    
    
    Queue<ProceduralLeg> legQueue = new Queue<ProceduralLeg>();
    
    //public SecondOrderDemo secondOrderDemo;
    void Awake()
    {
        previousPosition = this.transform.position;
        currentState = PAState.Idle;
    }

    // Start is called before the first frame update
    void Start()
    {



        //onEnter = false;
        //for(int i = 0; i < hasMoved.Length; i++)
       // {
        //    hasMoved[i] = false;
       //     currentlyUpdating[i] = false;
        //}
        //CreateTargets();
        //GetTarget();
        //UpdateStaticLegPos();
    }
    */

    // Update is called once per frame
   //void Update()
   // {
        /*currentSpeed = (this.transform.position - previousPosition).magnitude / Time.deltaTime;
        HandleIdle();
      

        if(scaleStepDistanceWithSpeed)
        {
            stepDistanceScalar = Mathf.Pow((1-rateOfGrowthFactor_stepDistance), currentSpeed); 
        }

        for (int i = 0; i < easyIK.Count; i++)
        {
            easyIK[i].ikTarget = tempTransforms[i];
            easyIK[i].SolveIK();
        }

        for (int i = 0; i < targetTransforms.Count; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(targetTransforms[i].position + rayCastYOffset, -Vector3.up, out hit, Mathf.Infinity, groundLayer))
            {
                // If the raycast hits something, set the target transform's Y position to the hit point's Y position
                Vector3 targetPosition = targetTransforms[i].position;
                targetPosition.y = hit.point.y;
                targetTransforms[i].position = targetPosition;
            }


            // If the current step is even, move the first and third legs
            // If the current step is odd, move the second and fourth legs
            /*if(easyIK[i].IsTargetReachable() == false)
            {
                MoveToTarget(i);
            }*/
            // if (Vector3.Distance(tempTransforms[i].position, targetTransforms[i].position) > maxDistance || easyIK[i].IsTargetReachable() == false)
            // Check if the distance between the tempTransform and the targetTransform is greater than maxDistance
            //if (Vector3.Distance(tempTransforms[i].position, targetTransforms[i].position) > (stepDistanceThreshold * stepDistanceScalar))
            //{
            //    UpdatePattern(i);
            //}

           

       /* }
            

        
        
        for(int i = 0; i < tempTransforms.Count; i++)
        {
            RaycastHit hit;
            if (Physics.Raycast(targetTransforms[i].position + rayCastYOffset, -Vector3.up, out hit, Mathf.Infinity, groundLayer))
            {
                // If the raycast hits something, set the target transform's Y position to the hit point's Y position
                Vector3 targetPosition = tempTransforms[i].position;
                targetPosition.y = hit.point.y;
                tempTransforms[i].position = targetPosition;
            }
        }
      


        previousPosition = this.transform.position;
    */
       
   // }
    /*
    public void GetTarget()
    {
        if(tempTransforms.Count == 0)
        {
            CreateTargets();
        }
        else
        {
            for (int i = 0; i < easyIK.Count; i++)
            {
                easyIK[i].ikTarget = tempTransforms[i];
            }
        }
    }
    public void HandleIdle()
    {
        bool isMoving = currentSpeed >= speedThreshold;

        if(currentState == PAState.Moving && isMoving == false)
        {
            for(int i = 0; i < tempTransforms.Count; i++)
            {
                UpdatePattern(i);
            }
        }

        currentState = isMoving ? PAState.Moving : PAState.Idle;
    }


    public void UpdatePattern(int i)
    {
        if(currentlyUpdating[i] == true) return;

        if((i==0 || i == 3) && (!update23))
        {
            if(onEnter == false)
            {
                onEnter = true;
                tempNum = currentStep;
                hasMoved[0] = false;
                hasMoved[3] = false;
            }

            update23 = false;
            update14 = true;

            if(hasMoved[i] == false && currentlyUpdating[i] == false)
            {
                // If it is, start the MoveToTarget coroutine
                currentlyUpdating[i] = true;
                StartCoroutine(MoveToTarget(i, () => {
                    hasMoved[i] = false;
                    currentlyUpdating[i] = false;
                }));
                
                hasMoved[i] = true;
            }
            
            if(currentStep % 2 == 0 && currentStep != tempNum)
            {
                update14 = false;
                update23 = true;
                onEnter = false;
            }
        }
        
        if((i==1 || i == 2) && (!update14))
        {
            if(onEnter == false)
            {
                onEnter = true;
                tempNum = currentStep;
                hasMoved[1] = false;
                hasMoved[2] = false;
            }

            update23 = true;
            update14 = false;

            if(hasMoved[i] == false && currentlyUpdating[i] == false)
            {
                // If it is, start the MoveToTarget coroutine
                currentlyUpdating[i] = true;
                StartCoroutine(MoveToTarget(i, () => {
                    hasMoved[i] = false;
                    currentlyUpdating[i] = false;
                }));
                hasMoved[i] = true;
            }

            if(currentStep % 2 == 0  && currentStep != tempNum)
            {
                update14 = true;
                update23 = false;
                onEnter = false;
            }
        }
    }


    //create instances of each of the target prefabs
    public void CreateTargets()
    {
        for (int i = 0; i < targetPrefab.Count; i++)
        {
            GameObject targetInstance = Instantiate(targetPrefab[i], Vector3.zero, Quaternion.identity);
            targetInstance.transform.position = targetTransforms[i].position;
            tempTransforms.Add(targetInstance.transform);
            easyIK[i].ikTarget = tempTransforms[i];
            secondOrderDynamics.Add(new SecondOrderDynamics(f, z, r, tempTransforms[i].position));
        }
    }
    void UpdateStaticLegPos()
    {
        for (int i = 0; i < footTransforms.Count; i++)
        {
            RaycastHit hit;
            /*if (Physics.Raycast(footTransforms[i].position, -Vector3.up, out hit, Mathf.Infinity, groundLayer))
            {
                // If the raycast hits something, set the corresponding target transform's position to the hit point
                tempTransforms[i].position = hit.point;
            }*/
        //}
    //}

    
    /*
    IEnumerator MoveToTarget(int index, Action callback)
    {
        coroutineIndex++;
        
        /*float duration = interpolationDuration; // Duration of the movement
        float elapsedTime = 0f; // Time that has passed
        //Vector3 startingPosition = tempTransform.position; // Starting position
        Vector3 midPoint = (tempTransforms[index].position + targetTransforms[index].position) / 2 + (Vector3.up * 0.5f); // Mid point above the starting and ending points


        //float distance = Vector3.Distance(tempTransforms[index].position, targetTransforms[index].position);
        bool canIncrementStep = true;
        bool targetMidPoint = true;

        float midPointDistance = Vector3.Distance(midPoint, targetTransforms[index].position);
        Vector3 prevTargetPosition = targetTransforms[index].position;

        //float duration = interpolationDuration;
        bool canIncrementStep = true;
        float elapsedTime = 0f;
        Vector3 midPoint = (tempTransforms[index].position + targetTransforms[index].position) / 2 + (Vector3.up * 0.5f);
        Vector3 prevTargetPosition = targetTransforms[index].position;

        bool reachedMidPoint = false;

        while (elapsedTime < individualStepDuration)
        {
            // Calculate the distance between temp and target
            float distance = Vector3.Distance(tempTransforms[index].position, targetTransforms[index].position);


            if(scaleFWithSpeed)
            {
                speedMultipler = Mathf.Pow((1+rateOfGrowthFactor_speed),currentSpeed);

                distanceMultipler = scaleFWithDistance ? Mathf.Pow((1 + rateOfGrowthFactor_distance), distance) : 1;

                secondOrderDynamics[index].SetConstants(f * (speedMultipler * distanceMultipler), z, r);
            }

            if(elapsedTime >= timeBetweenSteps)
            {
                if(canIncrementStep)
                {
                    canIncrementStep = false;
                    currentStep++;
                } 
            }
           

            
            // Recalculate midpoint if target position changes
            //midPoint -= (prevTargetPosition - targetTransforms[index].position);
            midPoint = targetTransforms[index].position + (Vector3.up * midPointHeight);

            // Check if the temp has reached the midpoint
            if(!reachedMidPoint) reachedMidPoint = Vector3.Distance(tempTransforms[index].position, midPoint) <= midPointDistanceThreshold;
            //Debug.Log($"Distance to midpoint: {Vector3.Distance(tempTransforms[index].position, midPoint)}");

            if(elapsedTime > (individualStepDuration * reachMidPointTimeThreshold) && !reachedMidPoint)
            {
                Debug.LogError("Cannot reach midpoint");
                reachedMidPoint = true;
            }

            // Calculate adjusted speed based on distance
            float adjustedSpeed = Mathf.Clamp(distance, 1.0f, Mathf.Infinity);

            // Move towards midpoint if not reached yet, else move towards target
            Vector3 targetPosition = reachedMidPoint ? targetTransforms[index].position : midPoint;
            tempTransforms[index].position = secondOrderDynamics[index].Update(Time.deltaTime * adjustedSpeed, targetPosition, tempTransforms[index].position);

            // Increment step if conditions met
            if (elapsedTime >= timeBetweenSteps && reachedMidPoint)
            {
                //currentStep++;
                //elapsedTime = 0f; // Reset elapsed time for the next step
            }

            // Update elapsed time and distance
            elapsedTime += Time.deltaTime;
            distance = Vector3.Distance(tempTransforms[index].position, targetTransforms[index].position);
            prevTargetPosition = targetTransforms[index].position;

            // Debug drawing for visualization
            Debug.DrawRay(tempTransforms[index].position, targetTransforms[index].position - tempTransforms[index].position, Color.blue);
            Debug.DrawRay(midPoint, Vector3.up, Color.magenta);
            

            //Debug.Log($"Time: {elapsedTime}, Index: {index}");
            // Wait for the next frame
            //yield return null;
            yield return new WaitForSeconds(updateInterval);
        }

        if(elapsedTime >= individualStepDuration)
        {
            currentlyUpdating[index] = false;
            //Debug.Log("Coroutine " + coroutineIndex + " finished");
        }
        
        
        // Ensure the tempTransform ends at the target position
        //tempTransforms[index].position = targetTransforms[index].position;

        // Call the callback function when the coroutine has finished
        callback();
        
    }
    */
}
