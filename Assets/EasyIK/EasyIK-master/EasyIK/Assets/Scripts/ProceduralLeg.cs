using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

[System.Serializable]
public class ProceduralLeg
{
    public enum TargetState
    {
        Idle,
        MidPoint,
        EndTarget,
    }

    [TabGroup("tab1", "General", TextColor = "green")]  public int legIndex;
    [TabGroup("tab1", "General")] public Transform baseTransform;
    [TabGroup("tab1", "General")] public Transform footTransform;
    [TabGroup("tab1", "General")] public Transform lastJointTransform;
    [TabGroup("tab1", "General")] public GameObject targetPrefab;
    [TabGroup("tab1", "General")] public EasyIK easyIK;
    [TabGroup("tab1", "General")] public Transform targetTransform;

    [Title("Step Action:")]
    [TabGroup("tab1", "General")] public float stepDistanceThreshold;
    [TabGroup("tab1", "General")] public float stepDuration;
    [TabGroup("tab1", "General")] public float nextLegCallbackTime;

    [Title("Mid Point:")]
    [TabGroup("tab1", "General")] public float midPointHeight;
    [TabGroup("tab1", "General")] public float midPointDistanceThreshold;
    [TabGroup("tab1", "General")] public float rateOfGrowthFactor_speed;
    [TabGroup("tab1", "General")] public float exponentialGrowthoffset;
    [TabGroup("tab1", "General")] [Range(0,1)] public float abandonMidPointTimeThreshold;

    [Title("Ground Check:")]
    [TabGroup("tab1", "General")] public float groundCheckRadius;
  


    [Title("States:")]
    [TabGroup("tab1", "Dynamic", TextColor = "blue")] [ReadOnly] public bool isMoving;
    [TabGroup("tab1", "Dynamic", TextColor = "blue")] [ReadOnly] public bool isFootGrounded;
    [TabGroup("tab1", "Dynamic", TextColor = "blue")] [ReadOnly] public bool reachedMidPoint;
    [TabGroup("tab1", "Dynamic", TextColor = "blue")] [ReadOnly] public float distanceToTarget;
    [TabGroup("tab1", "Dynamic")] [ReadOnly] public Transform tempTargetTransform;
    [TabGroup("tab1", "Dynamic")] [ReadOnly] public Vector3 midPoint;

    
    [Title("Second Order Dynamics:")]
    [TabGroup("tab1", "Dynamic")] [ReadOnly] public Vector3 currentSODInputPosition;
    [TabGroup("tab1", "Dynamic")] [ReadOnly] public Vector3 currentTargetPosition;
    [TabGroup("tab1", "Dynamic")] [ReadOnly] public Vector3 SODynamicsOutputPosition;
    
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] public SecondOrderDynamics secondOrderDynamics;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] public TargetState targetState;

    
    
    [TabGroup("tab1","VFXEvents", TextColor = "purple")] [SerializeField] public List<VisualEffectStruct> vfxStructs = new List<VisualEffectStruct>();
    [TabGroup("tab1","VFXEvents", TextColor = "purple")] [SerializeField] [ReadOnly] public VFXSpawner vfxSpawner;



    public void OnMoveLeg()
    {
        targetState = TargetState.MidPoint;
        isMoving = true;
    }

    public void IsFootGrounded(LayerMask groundLayer)
    {
        
        // Check if the foot is touching the ground
        bool grounded = Physics.CheckSphere(footTransform.position, groundCheckRadius, groundLayer);

        if(!isFootGrounded && grounded)
        {
            foreach(VisualEffectStruct vfx in vfxStructs)
            {
                VFXEventController.Instance.SpawnSimpleVFXGeneral(vfx, footTransform, null);
            }
           
            //VFXEventController.Instance.SpawnVFXWorldProceduralLeg(legIndex,0);
            //VfxSpawner.Instance.;
        }

        isFootGrounded = grounded;
        
    }

    public void UpdateDistance()
    {
        if (tempTargetTransform != null && targetTransform != null)
        {
            distanceToTarget = Vector3.Distance(tempTargetTransform.position, targetTransform.position);
            //Debug.DrawRay(tempTargetTransform.position, Vector3.up * 10, Color.red);
        }
        else
        {
            distanceToTarget = 0f;
        }
    }

    public void UpdateMidPoint(float currentSpeed)
    {
        if (tempTargetTransform != null && targetTransform != null)
        {
            float speedFactor = Mathf.Pow((1 + rateOfGrowthFactor_speed), currentSpeed) + exponentialGrowthoffset;
            midPoint = new Vector3(targetTransform.localPosition.x,targetTransform.position.y, targetTransform.localPosition.z) + (Vector3.up * midPointHeight * speedFactor);
        }
        else
        {
            midPoint = Vector3.zero;
        }
    }

    public void UpdateReachedMidpoint(float elapsedTime)
    {
        if (tempTargetTransform != null && targetTransform != null)
        {
            // Check if the temp has reached the midpoint
            if(!reachedMidPoint) reachedMidPoint = Vector3.Distance(CastTempToLocalSpace(), midPoint) <= midPointDistanceThreshold;

            if(reachedMidPoint)
            {
                targetState = TargetState.EndTarget;
            }
        }

        if(elapsedTime > (stepDuration * abandonMidPointTimeThreshold) && !reachedMidPoint)
        {
            Debug.LogError("Cannot reach midpoint");
            reachedMidPoint = true;
            targetState = TargetState.EndTarget;
        }
    }

    public Vector3 CastTempToLocalSpace()
    {
        if (tempTargetTransform != null && targetTransform != null)
        {
            
            Vector3 localTempPos = baseTransform.InverseTransformPoint(tempTargetTransform.position);
            //Debug.Log(localTempPos);
            return localTempPos;
        }
        else
        {
            return Vector3.zero;
        }
    }



}
