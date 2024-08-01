using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondOrderDemoPositionGun : MonoBehaviour
{
    public UpdateMode updateMode;
    public float updateInterval = 0.01f; // Set your desired update interval here
    public float f = 5;
    public float z = 1.0f;
    public float r = 0.0f;

    public Transform target;
    public Transform baseTransform;
    

    public SecondOrderDynamics secondOrderDynamics;

    [SerializeField] private int coroutineCount = 0;
    [SerializeField] Vector3 tempTargetPosition;
    [SerializeField] Vector3 Pos_WS;

    // Start is called before the first frame update
    void Awake()
    {
        secondOrderDynamics = new SecondOrderDynamics(f, z, r, transform.position);
    }

    public void CustomStart()
    {
        StartCoroutine(CustomUpdate());
    }

    public void RecalculateConstants()
    {
        secondOrderDynamics.SetConstants(f, z, r);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(updateMode == UpdateMode.Update)
        {
            transform.position = secondOrderDynamics.Update(Time.fixedDeltaTime, target.position, transform.position);
        }

        
    }

    void FixedUpdate()
    {
        if(updateMode == UpdateMode.FixedUpdate)
        {
            transform.position = secondOrderDynamics.Update(Time.fixedDeltaTime, target.position, transform.position);
        }
    }

    public void RecalculatePosition()
    {
        if(updateMode == UpdateMode.CustomUpdate)
        { 
            // Calculate the target position relative to the baseTransform
            Vector3 targetPosition = baseTransform.InverseTransformPoint(target.position);

            Vector3 localPos = baseTransform.InverseTransformPoint(transform.position);
            tempTargetPosition = secondOrderDynamics.Update(Time.deltaTime, targetPosition, localPos);
        }
    }

    private IEnumerator CustomUpdate()
    {
        coroutineCount++;
        while (true)
        {
            //Debug.Log("Running CustomUpdate. UpdateMode: " + updateMode + ", UpdateInterval: " + updateInterval);
            //Debug.Log("target is " + (target == null ? "null" : "not null"));

            if(updateMode == UpdateMode.CustomUpdate)
            {   
                // Calculate the target position relative to the baseTransform
                Vector3 targetPosition = baseTransform.InverseTransformPoint(target.position);

                //Debug.Log("Custom Update 1");
                Vector3 localPos = baseTransform.InverseTransformPoint(transform.position);
                tempTargetPosition = secondOrderDynamics.Update(Time.deltaTime, targetPosition, localPos);

                // Convert the updated position back to world space
                Pos_WS = baseTransform.TransformPoint(tempTargetPosition);
                transform.position = Pos_WS;
            }
    
            yield return new WaitForSeconds(updateInterval);
        }
    }
}


    /*
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


            // Convert the updated position back to world space
            Vector3 Pos_WS = legs[index].baseTransform.TransformPoint(legs[index].SODynamicsOutputPosition);
            legs[index].tempTargetTransform.position = Pos_WS;

            // Update elapsed time and distance
            elapsedTime += Time.deltaTime;
            prevTargetPosition = legs[index].targetTransform.position;
            
            // Calculate the distance between temp and target
            legs[index].UpdateDistance();
            legs[index].IsFootGrounded(groundLayer);
            

            yield return new WaitForSeconds(updateInterval);
    */
    

