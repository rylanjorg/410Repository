using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponCloner
{
    public Weapon CloneWeapon(Weapon original, Action<GameObject> weaponModelCallback)
    {
        Weapon copy = (Weapon)original.ShallowCopy();

        copy.RootTransformData = original.RootTransformData.Clone();
        copy.WeaponRoot = WeaponCreationUtility.CreateRootWeaponObject(copy.RootTransformData, copy.WeaponName, null);
        copy.WeaponAnimation = original.WeaponAnimation;

        if(original.weaponData.addAnimator)
        {
            WeaponCreationUtility.AddAnimator(copy.WeaponRoot, copy.WeaponAnimation);

            if(copy.WeaponRoot.GetComponent<Animator>() != null && copy.WeaponAnimation != null) 
                copy.Animator = copy.WeaponRoot.GetComponent<Animator>();
            else 
                Debug.LogError("Failed to add weapon animator on weapon clone" + copy.WeaponAnimation + "," + copy.WeaponRoot.GetComponent<Animator>());
        }
        
        copy.MyAudioSource = copy.WeaponRoot.AddComponent<AudioSource>();

        /*copy.AttackData = new List<AttackData>();
        foreach (AttackData attack in original.AttackData)
        {
            AttackData attackDataClone = attack.Clone();
            attackDataClone.attack.OnAwake(copy, attackDataClone);
            copy.AttackData.Add(attackDataClone);
        }

        if(copy.WeaponModelPrefab == null)
        {
            Debug.LogError("WeaponModelPrefab is null");
            return null;
        }
        
       

        foreach(AttackData attack in copy.AttackData)
        {
            foreach(CooldownEventTrigger trigger in attack.SimpleCooldown.CooldownEventTriggers)
            {
                trigger.visualEffectStruct.parentTransform_ = WeaponExtensions.FindTransformInHierarchy(weaponModelInstance.transform, trigger.visualEffectStruct.parentTransformName_);
            }
        }*/
         GameObject weaponModelInstance = GameObject.Instantiate(copy.WeaponModelPrefab);
       
        if(weaponModelInstance == null)
        {
            Debug.LogError("WeaponModelInstance is null");
            return null;
        }
        
        if(copy.UseOutline == true)
        {
            weaponModelInstance.layer = LayerMask.NameToLayer("Outline");
        }

        weaponModelInstance.transform.parent = copy.WeaponRoot.transform;
        weaponModelInstance.transform.localPosition = Vector3.zero;
        weaponModelInstance.transform.localRotation = Quaternion.identity;
        weaponModelInstance.transform.localScale = original.WeaponModelPrefab.transform.localScale;
        copy.WeaponModelInstance = weaponModelInstance;

        weaponModelInstance.transform.localRotation = copy.RootTransformData.rotation;

        weaponModelCallback?.Invoke(weaponModelInstance);

        WeaponModel weaponController = copy.WeaponRoot.GetComponent<WeaponModel>();

        if(weaponController != null)
        {
            weaponController.weaponSettingsInstance = copy;
        }
        else
        {
            Debug.LogError("Could not find Weapon Model -> is null");
            return null;
        }

        return copy;
    }
}
