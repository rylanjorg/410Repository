using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponCreationHandler
{
    // Define an event that is triggered when a weapon is created
    public event Action<GameObject> OnWeaponCreated;

    public GameObject CreateWeapon(Weapon weaponData, Transform spawnLocationTransform, Transform parentTransform, ISimpleWeaponObject simpleWeaponObject)
    {
        if (weaponData.WeaponModelPrefab != null)
        {
            GameObject weaponInstance = GameObject.Instantiate(weaponData.WeaponModelPrefab);
            weaponData.WeaponModelInstance = weaponInstance;
            weaponData.WeaponModelInstance.transform.localScale = weaponData.RootTransformData.scale;
            weaponData.WeaponModelInstance.transform.parent = parentTransform;

            if(weaponInstance == null)
            {
                Debug.LogError("WeaponSettingsInstance is null after clone()");
                return null;
            }


          
            weaponData.WeaponRoot = WeaponCreationUtility.CreateRootWeaponObject(weaponData.RootTransformData, weaponData.WeaponName, null);
            


            weaponData.MyAudioSource = weaponData.WeaponRoot.AddComponent<AudioSource>();
            Vector3 newPosition = weaponData.WeaponRoot.transform.position + spawnLocationTransform.position;

            weaponData.WeaponRoot.transform.position = newPosition;
            //weaponData.WeaponRoot.transform.parent = parentTransform;
            weaponData.WeaponRoot.layer = 10;

            //weaponInstance.transform.parent = weaponData.WeaponRoot.transform;
            weaponData.WeaponRoot.transform.parent = weaponInstance.transform;
            weaponInstance.transform.localPosition = Vector3.zero;
            weaponData.WeaponRoot.transform.localPosition = Vector3.zero;

            weaponData.RootTransform = weaponData.WeaponRoot.transform;



            List<AttackData> attackDataList = new List<AttackData>();
            foreach(AttackData attackData in weaponData.AttackData)
            {
                AttackData newAttackData = new AttackData(attackData.attack, weaponData);
                newAttackData.attack.OnAwake(newAttackData.attackRuntimeData);
                newAttackData.attackRuntimeData.attackID = simpleWeaponObject.GetNextAvailiableAttackID();
                newAttackData.attackRuntimeData.SetUI();
                attackDataList.Add(newAttackData);
   
            }
            weaponData.AttackData = attackDataList;


            // Invoke the OnWeaponCreated event
            OnWeaponCreated?.Invoke(weaponData.WeaponRoot);
      
            return weaponData.WeaponRoot;
        }
        else
        {
            Debug.LogError("WeaponModel prefab is null! on spawn weapon");
            return null;
        }
    }


    
}
