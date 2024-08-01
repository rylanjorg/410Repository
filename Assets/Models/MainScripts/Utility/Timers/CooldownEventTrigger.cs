 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

[System.Serializable]
public class CooldownEventTrigger
{

    [BoxGroup("Event")] [SerializeField] [HideLabel] public VisualEffectStruct visualEffectStruct;
 
    private bool hasTriggered;

    public bool HasTriggered
    {
        get { return hasTriggered; }
        set { hasTriggered = value; }
    }


    public CooldownEventTrigger()
    {
        // Subscribe to the event
        //ClassThatTriggersTheEvent.OnWeaponInstanceSet += SetWeaponInstance;
    }

   

    public CooldownEventTrigger(VisualEffectStruct visualEffectStruct, Weapon weaponInstance)
    {
        //this.visualEffectStruct = visualEffectStruct.Clone();
        this.hasTriggered = false;
        
    }

    public void SetWeaponInstance(Weapon weapon)
    {
        // Set the weapon instance when the event is triggered
       // this.weaponInstance = weapon;
    }

    public void CheckAndTriggerEvent(float cooldownProgress, AttackRuntimeData attackRuntimeData)
    {    
        if(this.visualEffectStruct.VfxPrefab == null)
        {
            Debug.LogError($"VFX prefab is null");
            return;
        }

        if (visualEffectStruct == null || visualEffectStruct.VfxPrefab == null || visualEffectStruct.transformData == null || attackRuntimeData.weaponInstance == null)
        {
            Debug.LogError($"One of the required fields is null: visualEffectStruct: [{visualEffectStruct}], vfx: [{visualEffectStruct.VfxPrefab}], transformData: [{visualEffectStruct.transformData}]");
            return;
        }

        //Debug.Log($"Checking and triggering event with cooldownProgress: {cooldownProgress}, timeOffset: {this.visualEffectStruct.timeOffset}");

        if (cooldownProgress >= this.visualEffectStruct.spawnTimeOffset && !hasTriggered && !visualEffectStruct.dontSpawnOnCooldown)
        {
            /*VFXEventController.Instance.SpawnSimpleVFXGeneral(vfxListHolder.vfxStructs[onWalkPSIndex], weaponInstance.RootTransform);       
            // Instantiate the VFX at the specified transform
            GameObject vfxInstance = Object.Instantiate(visualEffectStruct.VfxPrefab, weaponInstance.RootTransform.position, Quaternion.identity);
            vfxInstance.transform.SetParent(weaponInstance.RootTransform); // Set the weapon's RootTransform as the parent
            vfxInstance.transform.localPosition = visualEffectStruct.transformData.position;
            vfxInstance.transform.localRotation = visualEffectStruct.transformData.rotation;
            vfxInstance.transform.localScale = visualEffectStruct.transformData.scale;*/

            //Transform _parentTransform = attackRuntimeData.visualEffectData.parentTransforms[attackRuntimeData.attackID];
            Transform _parentTransform = attackRuntimeData.visualEffectData.parentTransforms[0];
            VFXEventController.Instance.SpawnSimpleVFXGeneral(visualEffectStruct, attackRuntimeData.weaponInstance.RootTransform, _parentTransform); 

            // Mark the event as triggered so it doesn't get triggered again
            hasTriggered = true;
        }
    }

    public void OverrideSpawnVFX(AttackRuntimeData attackRuntimeData)
    {
        //Transform _parentTransform = attackRuntimeData.visualEffectData.parentTransforms[attackRuntimeData.attackID];
        Transform _parentTransform = attackRuntimeData.visualEffectData.parentTransforms[0];
        VFXEventController.Instance.SpawnSimpleVFXGeneral(visualEffectStruct, attackRuntimeData.weaponInstance.RootTransform, _parentTransform); 
    }

    /*public CooldownEventTrigger Clone()
    {
        CooldownEventTrigger clone = new CooldownEventTrigger();

        VisualEffectStruct visualEffectStruct = new VisualEffectStruct(this.visualEffectStruct.VfxPrefab, this.visualEffectStruct.spawnTimeOffset);
        clone.visualEffectStruct = this.visualEffectStruct.Clone();
        clone.hasTriggered = this.hasTriggered;
        clone.weaponInstance = this.weaponInstance; // Assuming weaponInstance should also be cloned
        Debug.Log("cloned vfxstuct " + clone.visualEffectStruct.name + " : " + clone.visualEffectStruct.instanceSpace + " : " + clone.visualEffectStruct.deathTime + " : " + clone.visualEffectStruct.vfxDestroyMode); // This should print the name of the prefab (if it's not null
        Debug.Log(clone.visualEffectStruct.VfxPrefab);
        return clone;
    }*/
}