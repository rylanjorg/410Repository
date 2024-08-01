using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationBlender : MonoBehaviour
{
    public PlayerWeaponContainer playerWeaponContainer;
    public WeaponIK weaponIK;
    public float startDelay = 1.0f;
    public Transform targetTransform1; // First target transform
    public Transform targetTransform2; // Second target transform
    public float blendSpeed01 = 1f; // Blend speed
    public float blendSpeed10 = 1f; // Blend speed
    [Range(0f, 1f)] public float weight = 1.0f; // Blend factor
    [Range(0f, 1f)] public float targetWeight = 0.5f; // Blend factor
    [Range(0f, 1f)] public float activeBlendFactor = 0.2f;
    public bool breakCoroutine = false;
    public Coroutine currentBlendCoroutine;
    public bool isCoroutineRunning;

    public int activeCoroutineCount = 0;
    public bool canAimFlag;
    

    void Awake()
    {
        isCoroutineRunning = false;
        weight = 1.0f;
        breakCoroutine = false;
        canAimFlag = false;
    }

    void Update()
    {
        canAimFlag = false;
        //isCoroutineRunning = currentBlendCoroutine != null ? true : false;
        breakCoroutine = false;
        float previousWeight = targetWeight;
        targetWeight = (playerWeaponContainer.currentWeaponState == PlayerWeaponContainer.WeaponState.HipFire || playerWeaponContainer.currentWeaponState == PlayerWeaponContainer.WeaponState.AimDownSights)  ? activeBlendFactor : 1.0f;

        // Ensure both target transforms are not null
        if (targetTransform1 == null || targetTransform2 == null)
        {
            Debug.LogWarning("One or both target transforms are not assigned!");
            return;
        }

        if(targetWeight != previousWeight && targetWeight == activeBlendFactor)
        {
            Debug.Log("targetWeight == activeBlendFactor");
            BlendAim1To0();
        }
        else if(targetWeight != previousWeight && previousWeight == activeBlendFactor) 
        {
            BlendAim0To1();
            //weight = targetWeight;
        }

        if(activeCoroutineCount == 1)
        {
            weaponIK.weaponAnimationHelper.StartDelay(startDelay);
        }
        /*
        else if(targetWeight != previousWeight && previousWeight == 1.0f)
        {
            //breakCoroutine = true;
            //weight = targetWeight;
            //BlendAim1To0();
        }*/

        // Calculate the blended rotation using the blend factor
        Quaternion blendedRotation = Quaternion.Lerp(targetTransform1.rotation, targetTransform2.rotation, weight);

        // Extract the Euler angles from the blended rotation
        Vector3 blendedEulerAngles = blendedRotation.eulerAngles;

        // Set the X and Z components to the current rotation's X and Z components
        blendedEulerAngles.x = transform.rotation.eulerAngles.x;
        blendedEulerAngles.z = transform.rotation.eulerAngles.z;

        // Convert back to a Quaternion
        blendedRotation = Quaternion.Euler(blendedEulerAngles);

        // Apply the blended rotation to the current transform
        transform.rotation = blendedRotation;
    }

    public void BlendAim0To1()
    {
        //isCoroutineRunning = currentBlendCoroutine == null ? true : false;
        // Stop the previous coroutine
        if (isCoroutineRunning)
        {
            StopCoroutine(currentBlendCoroutine);
             isCoroutineRunning = false; // Update the flag
            activeCoroutineCount = Mathf.Max(activeCoroutineCount - 1, 0);
        }

        // Start the new coroutine and keep a reference to it
        if(activeCoroutineCount < 1) 
        {
            currentBlendCoroutine = StartCoroutine(BlendAim01());
            isCoroutineRunning = true; // Update the flag
        }
        
    }

    public void BlendAim1To0()
    {
        //isCoroutineRunning = currentBlendCoroutine == null ? true : false;
        // Stop the previous coroutine
        if (isCoroutineRunning)
        {
            StopCoroutine(currentBlendCoroutine);
            isCoroutineRunning = false; // Update the flag
            activeCoroutineCount = Mathf.Max(activeCoroutineCount - 1, 0);
        }

        // Start the new coroutine and keep a reference to it
        if(activeCoroutineCount < 1)
        {   
            currentBlendCoroutine = StartCoroutine(BlendAim10());
            isCoroutineRunning = true; // Update the flag
        } 
    }


    IEnumerator BlendAim01()
    {
        canAimFlag = false;
        activeCoroutineCount++;
        float blendDuration = 1f / blendSpeed01; // Calculate the total duration of the blend
        float elapsedTime = 0f; // Track the elapsed time

        while (elapsedTime < blendDuration)
        {
           
            float t = elapsedTime / blendDuration;
            weight = Mathf.SmoothStep(activeBlendFactor, 1f, t);
            // Apply the weight to your animation
            // For example: animator.SetFloat("Blend", weight);

            elapsedTime += Time.deltaTime;
            yield return null;
            if(breakCoroutine) break;
            if(targetWeight < weight) break;
        }


        weight = 1f;

        breakCoroutine = false;
        // Apply the final weight to your animation
        // For example: animator.SetFloat("Blend", weight);
        isCoroutineRunning = false; // Update the flag
        activeCoroutineCount--;
        canAimFlag = true;
    }

    IEnumerator BlendAim10()
    {
        canAimFlag = false;
        activeCoroutineCount++;
        float blendDuration = 1f / blendSpeed10; // Calculate the total duration of the blend
        float elapsedTime = 0f; // Track the elapsed time

        while (elapsedTime < blendDuration)
        {
            float t = elapsedTime / blendDuration;
            weight = Mathf.SmoothStep(1f, activeBlendFactor, t);
            // Apply the weight to your animation
            // For example: animator.SetFloat("Blend", weight);

            elapsedTime += Time.deltaTime;
            yield return null;
            if(breakCoroutine) break;
           
        }

        weight = activeBlendFactor;

        breakCoroutine = false;
        // Ensure the weight is exactly 0 at the end
        
        // Apply the final weight to your animation
        // For example: animator.SetFloat("Blend", weight);
        isCoroutineRunning = false; // Update the flag
        activeCoroutineCount--;
        canAimFlag = true;
    }

}
