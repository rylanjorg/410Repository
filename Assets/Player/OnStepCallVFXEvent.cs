using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnStepCallVFXEvent : MonoBehaviour
{
    public Transform leftFoot;
    public Transform rightFoot;
    public LayerMask groundLayer; // Layer to check for groun
    public float groundCheckRadius = 0.2f; // Radius of the sphere used to check for ground
    public PlayerDataManagement playerDataManagement;
    public float velocityInfluence = 0.2f;
    public float Offsety = -0.1f;

    private bool leftFootWasGrounded;
    private bool rightFootWasGrounded;

    public int onStepPs;
    public VFXListHolder vfxListHolder;
    private void Update()
    {
        CheckFoot(leftFoot, ref leftFootWasGrounded);
        CheckFoot(rightFoot, ref rightFootWasGrounded);
    }

    private void CheckFoot(Transform foot, ref bool wasGrounded)
    {
        bool isGrounded = Physics.CheckSphere(foot.position, groundCheckRadius, groundLayer);

        

        if (!wasGrounded && isGrounded)
        {
            Vector3 offsettedSpawnPos = foot.position + new Vector3((playerDataManagement.characterVelocityNormalized * playerDataManagement.currentHorizontalSpeed_Projected).x, 0 , (playerDataManagement.characterVelocityNormalized * playerDataManagement.currentHorizontalSpeed_Projected).z) * velocityInfluence;
            // Raycast down to get the ground position
            RaycastHit hit;
            if (Physics.Raycast(foot.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
            {
                offsettedSpawnPos.y = hit.point.y + Offsety;
            }


            // Foot has just touched the ground
            //vfxSpawner.SpawnVFXWorld(0); // Replace 0 with the index of the VFX you want to spawn
            //Debug.LogError("VFXEventController spawn footstep");
            

            // Create a new GameObject and set its position to the offset position
            GameObject vfxSpawnPoint = new GameObject("VFX Spawn Point");
            vfxSpawnPoint.transform.position = offsettedSpawnPos + vfxListHolder.vfxStructs[onStepPs].transformData.position;

            // Pass the Transform of the new GameObject to the SpawnSimpleVFXWorld method
            VFXEventController.Instance.vfxSpawner.SpawnSimpleVFXWorld(vfxListHolder.vfxStructs[onStepPs], vfxSpawnPoint.transform, null);

            // Optionally, destroy the new GameObject after the VFX has been spawned
            Destroy(vfxSpawnPoint);
        }

        wasGrounded = isGrounded;
    }
}
