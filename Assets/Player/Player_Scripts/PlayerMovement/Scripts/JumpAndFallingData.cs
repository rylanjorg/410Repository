using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

[CreateAssetMenu(menuName = "ScriptableObject/FSM/JumpAndFallAction")]
public class JumpAndFallingData : ScriptableObject
{
    public List<VisualEffectStruct> onJumpVFX = new List<VisualEffectStruct>();
    // Base Jump Action
    [Title("Base Jump Action:")]
    [TabGroup("tab1", "JumpingState", TextColor = "orange")] public float baseJumpHeight = 125f;
    [TabGroup("tab1", "JumpingState")] public AnimationCurve jumpCurve;
    [TabGroup("tab1", "JumpingState")] public float initialVerticalVelocity = 2f;
    [TabGroup("tab1", "JumpingState")] public float jumpCooldown = 0.50f;
           
    

    // Force Jump State
    [Title("Force Jump State:")]
    [TabGroup("tab1", "JumpingState")]  public float forceJumpDuration = 0.1f;


    // Extend Jump Settings
    [Title("Extend Jump State:")]
    [TabGroup("tab1", "JumpingState")]  public float maxJumpDuration = 0.5f;


    // Falling State
    [Title("Falling Action:")]
    [TabGroup("tab1", "FallingState", TextColor = "orange")] public float gravity = -15.0f;
    [TabGroup("tab1", "FallingState")] public float FallTimeout = 0.15f;
    [TabGroup("tab1", "FallingState")] [SerializeField] public float terminalVelocity = 53.0f;
    

    // Coyote Time
    [Title("Coyote Time Action:")]
    [TabGroup("tab1", "CoyoteTime", TextColor = "orange")] public float coyoteTime = 0.1f;
    
    
    // Jump And Falling Horizontal Movement:
    [Title("Horizontal Movement Action:")]
    [TabGroup("tab1", "JumpAndFallingHorizontalMovement", TextColor = "orange")] public bool updateSpeedWhileInAir = true;
    [TabGroup("tab1", "JumpAndFallingHorizontalMovement")] public float rotationSmoothTime = 0.1f;
    [TabGroup("tab1", "JumpAndFallingHorizontalMovement")] public float targetSpeed = 4.0f;
    [TabGroup("tab1", "JumpAndFallingHorizontalMovement")] public float speedOffset = 0.0f;
    [TabGroup("tab1", "JumpAndFallingHorizontalMovement")] public float speedChangeRate_Accelerate = 3.0f;
    [TabGroup("tab1", "JumpAndFallingHorizontalMovement")] public SpeedUpdateMethod speedUpdateMethod_Accelerate = SpeedUpdateMethod.AccelerateIfLess;
    [TabGroup("tab1", "JumpAndFallingHorizontalMovement")] public float speedChangeRate_Decelerate = 0.5f;
    [TabGroup("tab1", "JumpAndFallingHorizontalMovement")] public SpeedUpdateMethod speedUpdateMethod_Decelerate = SpeedUpdateMethod.SlowIfGreater;
}
