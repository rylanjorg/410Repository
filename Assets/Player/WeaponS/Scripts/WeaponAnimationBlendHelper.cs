using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponAnimationBlendHelper 
{
    public WeaponIK weaponIK;
    [SerializeField] float blendSpeed = 1.0f;
    [SerializeField] float blendSpeedDecrease = 0.5f;
    [SerializeField] private float delayTimeRemaining = 0f;
    public float transitionDelayTime = 0.5f;
    public float targetWeight;
    private Coroutine blendCoroutine;

    public WeaponAnimationBlendHelper()
    {
        //this.weaponIK = weaponIK;
    }

    public void StartDelay(float delayTime)
    {
        delayTimeRemaining = delayTime;
    }
    public void StopDelay()
    {
        delayTimeRemaining = 0;
    }

    public void OnUpdate()
    {
        if (weaponIK.weight < targetWeight && delayTimeRemaining > 0)
        {
            delayTimeRemaining -= Time.deltaTime;
            return;
        }

        if (weaponIK.weight != targetWeight)
        {
            float currentBlendSpeed = weaponIK.weight > targetWeight ? blendSpeedDecrease : blendSpeed;
            weaponIK.weight = Mathf.Lerp(weaponIK.weight, targetWeight, Time.deltaTime * currentBlendSpeed);
        }
    }

    /*
    public void BlendAim0To1(float value, bool checkForAimFlag, bool overrideCurrentCoroutine = true)
    {
        if(blendCoroutine != null && overrideCurrentCoroutine)
        {
            CoroutineStarter.Instance.StopCoroutine(blendCoroutine);
        }

        if(checkForAimFlag)
        {
            if(weaponIK.rotationBlender.canAimFlag) blendCoroutine = CoroutineStarter.Instance.StartCoroutine(BlendAim01(transitionDelayTime));
        } 
        else
        {
            blendCoroutine = CoroutineStarter.Instance.StartCoroutine(BlendAim01(0));
        }
    }

    
    public void BlendAim1To0(float value, bool checkForAimFlag, bool overrideCurrentCoroutine = true)
    {
        if(blendCoroutine != null && overrideCurrentCoroutine)
        {
            CoroutineStarter.Instance.StopCoroutine(blendCoroutine);
        }

        if(checkForAimFlag)
        {
            if(weaponIK.rotationBlender.canAimFlag) blendCoroutine = CoroutineStarter.Instance.StartCoroutine(BlendAim10(transitionDelayTime));
        } 
        else
        {
            blendCoroutine = CoroutineStarter.Instance.StartCoroutine(BlendAim10(0));
        }
    }


    IEnumerator BlendAim01(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay);
        float blendDuration = 1f / blendSpeed; // Calculate the total duration of the blend
        float elapsedTime = 0f; // Track the elapsed time
        float initalWeight = weaponIK.weight;

        while (elapsedTime < blendDuration)
        {
            weaponIK.weight = Mathf.Lerp(initalWeight, 1f, elapsedTime / blendDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        weaponIK.weight = 1f;
    }

    IEnumerator BlendAim10(float delay = 0.0f)
    {
        yield return new WaitForSeconds(delay);
        float blendDuration = 1f / blendSpeed; // Calculate the total duration of the blend
        float elapsedTime = 0f; // Track the elapsed time
        float initialWeight = weaponIK.weight;

        while (elapsedTime < blendDuration)
        {
            weaponIK.weight = Mathf.Lerp(initialWeight, 0f, elapsedTime / blendDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        weaponIK.weight = 0f;
    }
    */
}