using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;

public class BurstFireModifier : AttackModifier
{
    
    Weapon weaponInstance;
    [Title("Burst Fire Modifier", "This modifier will allow the weapon to fire multiple shots in a burst.")]
    [SerializeField] private float burstCount = 1.0f;
    [SerializeField] private float individualBurstDelay = 1.0f;
    [SerializeField] private List<int> overrideVFXIndices = new List<int>();
    

    public BurstFireModifier()
    {
        Type = ModifierType.ChargeUp;
    }

    public override void InitializeAttackModifier(Attack attack, Weapon weaponInstance)
    {
        //Debug.Log("Initializing Attack Modifier (ChargeUp)");
        base.InitializeAttackModifier(attack, weaponInstance);
    }

    public override bool ApplyModifier(bool ignoreModifiers, Action<AttackRuntimeData> baseAttack, AttackRuntimeData runtimeData)
    {
        if(runtimeData.attackInstance == null)
        {
            Debug.LogError("Apply modifier Attack is null");
            return false;
        }
    
        Debug.Log("BurstFireModifier");
        CoroutineStarter.Instance.StartCoroutine(PerformBurstTypeAttack(baseAttack, runtimeData));
        return false;
    }

    private IEnumerator PerformBurstTypeAttack(Action<AttackRuntimeData> baseAttack, AttackRuntimeData runtimeData)
    {
        
        for(int i = 0; i < burstCount; i++)
        {
            yield return new WaitForSeconds(individualBurstDelay);
            if (baseAttack != null)
            { 
                baseAttack(runtimeData);
                runtimeData.CallFunctionForMatchingValues(runtimeData.allVFXIDs, overrideVFXIndices);
            }
            else
            {
                Debug.LogError("baseAttack is null");
            }
        }
    }


    public override void RemoveModifier(Attack attack)
    {
        // Decrease the time between shots
        // This is just an example, you'll need to implement this based on how your Attack class is defined
        //attack.TimeBetweenShots -= chargeUpTime;
    }

 

}
