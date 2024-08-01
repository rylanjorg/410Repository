using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/FSM/SlideAction")]
public class SlideActionData : ScriptableObject
{
    [Header("Inscribed")]
    public SpeedUpdateMethod speedUpdateMethod;
    public float baseSlideSpeed = 10.0f;
    
    public float slideResourceCost = 0.5f;

    [Header(" - GravityAccelerationDownSlope")]
    public bool useCustomGravity;
    public float gravity = 10.0f;

    public float maxGravityAdjustedSlideSpeed = 30.0f;
    public float gravityAccelerateSpeedChangeRate = 3.0f;


    [Header(" - SpeedChangeRateCurve")]
    public float speedChangeRate = 3.0f;
    public float speedOffset = 0.1f;


    [Header(" - PlayerRotation")]
    public float slideRotationSmoothTime = 0.4f;
    public float maxAngle = 15.0f;

 
    [Header("Cooldown_Slide")]
    public float slideCooldown = 1.0f;

   
    [Header("State_IsSliding")]
    public float slideDuration = 0.3f;



    [Header("State_InputLocked")]
    public float inputLockTime = 0.4f;
  
}
