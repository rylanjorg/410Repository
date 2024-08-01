using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponCreationUtility 
{
    public static GameObject CreateRootWeaponObject(TransformData transformData, string name, Transform parent)
    {
        GameObject newObject = new GameObject(name + "_Root");

        // Set the parent
        newObject.transform.parent = parent;
        
        // Apply the stored transform data to the new GameObject
        newObject.transform.localPosition= transformData.position;
        newObject.transform.localRotation = transformData.rotation;
        newObject.transform.localScale = transformData.scale;


        return newObject;
    }

    public static void AddAnimator(GameObject target, RuntimeAnimatorController animatorController)
    {
        target.AddComponent<Animator>();
        Animator newAnimator = target.GetComponent<Animator>();
        if(newAnimator != null)
        {
            newAnimator.runtimeAnimatorController = animatorController;
        }
        else
        {
            Debug.LogError("Animator is null -> AddAnimator (WeaponCreationUtility)");
        }
        
    }

    public static bool IsInAttackStates(AnimatorStateInfo stateInfo, List<string> attackStates)
    {
        foreach (string stateName in attackStates)
        {
            if (stateInfo.IsName(stateName))
            {
                return true;
            }
        }
        return false;
    }

}
