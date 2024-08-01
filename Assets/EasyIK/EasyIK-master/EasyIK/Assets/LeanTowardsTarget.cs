using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

public class LeanTowardsTarget : MonoBehaviour
{   
    public enum State
    {
        Active,
        Reset,
    }

    public enum SpeedScaleFunction
    {
        Linear,
        Exponential,
    }
    [TabGroup("tab1", "General")] public SpeedScaleFunction function = SpeedScaleFunction.Exponential; // Maximum angle
    [ShowIf("function", SpeedScaleFunction.Linear)]
    [TabGroup("tab1", "General")] public float m = 0.01f; // Maximum angle

    [ShowIf("function", SpeedScaleFunction.Exponential)]
    [TabGroup("tab1", "General")] public float r = 0.01f; // Maximum angle
    [TabGroup("tab1", "General")] public float b = -1f; // Maximum angle
    [TabGroup("tab1", "General")] public float maxAngle = 90f; // Maximum angle
    [TabGroup("tab1", "General")] public BodyPositionIK bodyPositionIK;

    [Title("Reset Settings")]
    [TabGroup("tab1", "General", TextColor = "green")] public float resetThresholdDistance = 1f;

    [ShowIf("function", SpeedScaleFunction.Exponential)] [SerializeField] [TabGroup("tab1", "General")] float r_Reset = 0.1f;
    [ShowIf("function", SpeedScaleFunction.Linear)] [SerializeField] [TabGroup("tab1", "General")] float m_Reset = 0.1f;
    [TabGroup("tab1", "General")] [SerializeField] float stopUpdateThresholdAngle = 1.0f; // Set this to the duration you want
    [TabGroup("tab1", "General")] [SerializeField] public Transform resetTarget;

    
    [Title("Reset Controller State Conditional")]
    [TabGroup("tab1", "General")] [SerializeField] ProceduralLegController controller;
    [TabGroup("tab1", "General")] [SerializeField] float idleDuration = 1.0f; // Set this to the duration you want
    

    [TabGroup("tab1", "Dynamic", TextColor = "blue")] [SerializeField]  float leanAngleX; 
    [TabGroup("tab1", "Dynamic")] [SerializeField]  float leanAngleZ;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] Vector3 startingDirection;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] Vector3 clampedDirection;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] Vector3 direction;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] Quaternion startingRotation;
    [Title("Interpolation between inital and target (0-1)")]
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] float interpolationFactor;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] float angle;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] public State currentState;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] private float idleStartTime;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] private bool distanceThreshold;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] private bool angleThreshold; 
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] private float angleToStart; 
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] private Quaternion leanRotation; 

     
    
    void Awake()
    {
        if(bodyPositionIK != null) 
        {
            startingDirection = new Vector3(transform.up.x, transform.up.y, transform.up.z) + bodyPositionIK.bodyRotation * Vector3.up;
        }
        else
        {
            startingDirection = new Vector3(transform.up.x, transform.up.y, transform.up.z);
        }
        
        startingRotation = Quaternion.FromToRotation(transform.up, startingDirection);
        currentState = State.Reset;
    }

    // based on the magnitude of the distance vector compute the angle to adjust the 
    // transform rotation. The transform rotation should lean in the direction of the the direction vector 
    // (normalizedDirection)
    void ComputeAngle(Transform target, float _r, float _m)
    {
        Debug.Log("ComputeAngle");
        currentState = State.Active;
        // Compute the direction from the object to the target
        direction = (target.position - transform.position).normalized;
        float distance = (target.position - transform.position).magnitude;

        // Calculate the interpolation factor based on the function type
        switch (function)
        {
            case SpeedScaleFunction.Linear:
                interpolationFactor = (_m * distance) + b;
                break;
            case SpeedScaleFunction.Exponential:
                interpolationFactor = Mathf.Pow((1 + _r), distance) + b;
                break;
            default:
                interpolationFactor = 0;
                break;
        }

        // Linearly interpolate between the object's current upward direction and the target direction
        Vector3 lerpedDirection = Vector3.Lerp(transform.up, direction, interpolationFactor); 

        // Compute the rotation that rotates the object's current upward direction to the lerped direction
        Quaternion rotation = Quaternion.FromToRotation(transform.up, lerpedDirection);

        // Compute the angle between the current rotation of the object and the target rotation
        float totalAngle = Quaternion.Angle(transform.rotation, transform.rotation * rotation);

        // Clamp the total angle
        float clampedTotalAngle = Mathf.Clamp(totalAngle, 0, maxAngle);

        // Compute the rotation axis
        Vector3 rotationAxis = Vector3.Cross(transform.up, lerpedDirection);

        // Compute the target rotation using the clamped total angle and the rotation axis
        Quaternion targetRotation = Quaternion.AngleAxis(clampedTotalAngle, rotationAxis);

        // Apply the target rotation to the object
        transform.rotation = targetRotation;

        clampedDirection = targetRotation * Vector3.up;

        // Compute the rotation angles
        leanAngleX = Mathf.Atan2(clampedDirection.y, clampedDirection.x) * Mathf.Rad2Deg - 90;
        leanAngleZ = 90 - (Mathf.Atan2(clampedDirection.y, clampedDirection.z) * Mathf.Rad2Deg);

        Quaternion finalRotation = Quaternion.AngleAxis(leanAngleX, transform.forward) *
                                   Quaternion.AngleAxis(leanAngleZ, Vector3.right);

        leanRotation = finalRotation;
    }



    public void DrawRay()
    {
        // Draw a ray to visualize the direction of the interpolated rotation
        Debug.DrawRay(transform.position, clampedDirection * 5f, Color.blue);
        Debug.DrawRay(transform.position, direction * 5f, Color.red);
        Debug.DrawRay(transform.position, startingDirection * 5f, Color.green);
    }

    public void LeanAndReset(Transform target)
    {
        // Calculate the distance to the target
        float distance = Vector3.Distance(transform.position, target.position);
        distanceThreshold = distance < resetThresholdDistance;

        // Calculate the angle between the current up direction and the starting direction
        angleToStart = Vector3.Angle(transform.up, startingDirection);
        angleThreshold = angleToStart > stopUpdateThresholdAngle;
        
        if(distanceThreshold)
        {
            idleStartTime = 0;
            //if(angleThreshold) ClampedResetAngle(target);
            if(angleThreshold) ComputeAngle(target, r_Reset, m_Reset);
        }
        else if((controller != null && controller.currentState == ProceduralLegController.State.Idle))
        {
            if(controller != null && controller.currentState == ProceduralLegController.State.Idle && idleStartTime == 0) idleStartTime = Time.time;

            if (Time.time - idleStartTime >= idleDuration) 
            {
                //if(angleThreshold) ClampedResetAngle(target);
                if(angleThreshold) ComputeAngle(target, r_Reset, m_Reset );

            }
            else
            {
                ComputeAngle(target, r, m);
                Debug.Log("ComputeAngle during reset");
            }
        }
        else
        {
            // If the controller is not in the idle state, reset the start time
            idleStartTime = 0;

            ComputeAngle(target, r, m);
        }

        if(bodyPositionIK != null) 
        {
            transform.rotation = leanRotation * bodyPositionIK.bodyRotation;
        }
        else
        {
            transform.rotation = leanRotation;
        }
        DrawRay();
    }
}
