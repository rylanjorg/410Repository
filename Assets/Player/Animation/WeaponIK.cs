using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

[System.Serializable]
public class HumanBone {
    public HumanBodyBones bone;
    public float weight;
}
public class WeaponIK : MonoBehaviour
{
    [TabGroup("tab1", "General")] public PlayerWeaponContainer playerWeaponContainer;
    [TabGroup("tab1", "General")] public RotationBlender rotationBlender;
    [TabGroup("tab1", "General")] public Transform targetTransform;
    [TabGroup("tab1", "General")] public WeaponAnimationBlendHelper weaponAnimationHelper = new WeaponAnimationBlendHelper();
    


    [TabGroup("tab1", "IK")] public int iteraitons = 10;
    [TabGroup("tab1", "IK")] public float angleLimit = 90f;
    [TabGroup("tab1", "IK")] public float distanceLimit = 1.5f;
    [TabGroup("tab1", "IK")] public HumanBone[] humanBones;
    [TabGroup("tab1", "IK")] Transform[] boneTransforms;



    [TabGroup("tab1", "Crosshair Aim")] [SerializeField] private new Camera camera;
    [TabGroup("tab1", "Crosshair Aim")] [SerializeField] private LayerMask layerMask;
    [TabGroup("tab1", "Crosshair Aim")] [SerializeField] float offsetDistance = 1.0f; 
    [TabGroup("tab1", "Crosshair Aim")] [SerializeField] float defaultDistance = 30.0f;
    [TabGroup("tab1", "Crosshair Aim")] [SerializeField] Vector3 cameraOffset = new Vector3(0, 0, 0);


    [TabGroup("tab1", "Dynamic")] public Transform aimTransform;
    [TabGroup("tab1", "Dynamic")] [Range(0,1)] public float weight = 1.0f;

    
    void Awake()
    {
        weaponAnimationHelper.weaponIK = this;
       // weaponAnimationHelper = 
    }

    void Start()
    {
        Animator animator = GetComponent<Animator>();
        boneTransforms = new Transform[humanBones.Length];
        for(int i = 0; i < boneTransforms.Length; i++){
            boneTransforms[i] = animator.GetBoneTransform(humanBones[i].bone);
        }
    }


    





    void UpdateTargetPos()
    {
        // Get the camera's position and forward direction
        //Camera camera = GetComponent<Camera>();
        Vector3 cameraPosition = camera.transform.position;
        Vector3 forward = camera.transform.forward;

        // Set a default position for the target
        Vector3 defaultPosition = cameraPosition + forward * defaultDistance; // Change this to your desired default position

        // Shoot a ray from the camera
        if (Physics.Raycast(cameraPosition, forward, out RaycastHit hit, float.MaxValue, layerMask))
        {
            // If the ray hits something, set the target position to the hit point
            targetTransform.position = hit.point;
        }
        else
        {
            // If the ray doesn't hit anything, set the target position to the default position
            targetTransform.position = defaultPosition;
        }
    }

    Vector3 GetTargetPosition()
    {
        Vector3 targetDirection = targetTransform.position - aimTransform.position;
        Debug.DrawRay(aimTransform.position, targetDirection, Color.green);
        Vector3 aimDirection = aimTransform.forward;    
        float blendOut = 0.0f;

        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if(targetAngle > angleLimit){
            Debug.Log("Blend out: " + blendOut);
            blendOut += (targetAngle - angleLimit) / 50f;
        }

        float targetDistance = targetDirection.magnitude;
        if(targetDistance < distanceLimit){
            Debug.Log("Blend out: " + blendOut);
            blendOut += distanceLimit - targetDistance;
        }

         Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        //Vector3 direction = Vector3.Slerp(aimDirection, targetDirection, blendOut);
        return aimTransform.position + direction;
    }

    void Update()
    {
        weaponAnimationHelper.OnUpdate();
    }

    void LateUpdate()
    {
       
        if(targetTransform == null || aimTransform == null || boneTransforms == null){
            return;
        }

        UpdateTargetPos();

        if(playerWeaponContainer.currentWeaponState == PlayerWeaponContainer.WeaponState.HipFire || playerWeaponContainer.currentWeaponState == PlayerWeaponContainer.WeaponState.AimDownSights){
            Vector3 targetPosition = GetTargetPosition();
            for(int i = 0; i < iteraitons; i++){
                for (int b = 0; b < boneTransforms.Length; b++){
                    Transform bone = boneTransforms[b];
                    float boneWeight = humanBones[b].weight * weight;
                    AimAtTarget(bone, targetPosition, boneWeight);
                }
            }    
        }

      
        //AimAtTarget(bone, targetPosition);
    }

    private void AimAtTarget(Transform bone, Vector3 targetPosition, float weight)
    {
         // Check if the bone is part of the upper body
        if (IsUpperBody(bone))
        {
            Vector3 aimDirection = aimTransform.forward;
            Vector3 targetDirection = targetPosition - aimTransform.position;
            Quaternion aimTowards = Quaternion.FromToRotation(aimDirection,targetDirection);
            Quaternion blendedRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
            bone.rotation = blendedRotation * bone.rotation;
        }
        else
        {
            // Follow the normal animation for the lower body
            // ...
        }
        
    }

    private bool IsUpperBody(Transform bone)
    {
        // Check if the bone is part of the upper body
        // This depends on your specific character model and may need to be adjusted
        return bone.name.Contains("Spine") || bone.name.Contains("Arm") || bone.name.Contains("Head");
    }

    private void OnDrawGizmos(){
        if(targetTransform != null){
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(targetTransform.position, 0.3f);
        }
    }
  

}
