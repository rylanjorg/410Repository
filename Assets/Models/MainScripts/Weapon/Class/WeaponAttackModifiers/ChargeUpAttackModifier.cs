using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;

[System.Serializable]
public class ChargeUpModifier : AttackModifier 
{
    [SerializeField]
    private float chargeUpTimeModifier = 1f;  // Renamed to chargeUpTimeModifier
    [SerializeField] private List<int> overrideVFXIndices = new List<int>();
    private List<int> temp = new List<int>();

    public ChargeUpModifier()
    {
        Type = ModifierType.ChargeUp;
    }

    public override void InitializeAttackModifier(Attack attackInstance, Weapon weaponInstance)
    {
        //Debug.Log("Initializing Attack Modifier (ChargeUp)");
        base.InitializeAttackModifier(attackInstance, weaponInstance);
        int i = 0;
        foreach(CooldownEventTrigger cet in attackInstance.SimpleCooldown.CooldownEventTriggers)
        {
            temp.Add(i);
            i++;
        }
    }

    public override bool ApplyModifier(bool ignoreModifiers, Action<AttackRuntimeData> baseAttack, AttackRuntimeData runtimeData)
    {
        CoroutineStarter.Instance.StartCoroutine(ChargeUp(baseAttack, runtimeData));
        return true;
    }

    private IEnumerator ChargeUp(Action<AttackRuntimeData> baseAttack, AttackRuntimeData runtimeData)
    {
        Debug.Log("ChargeUpModifierTime start");
        runtimeData.CallFunctionForMatchingValues(temp, overrideVFXIndices);
        yield return new WaitForSeconds(chargeUpTimeModifier);
        Debug.Log("ChargeUpModifierTime finished");
        baseAttack(runtimeData);
    }

    

    public override void RemoveModifier(Attack attack)
    {
        // Decrease the time between shots
        // This is just an example, you'll need to implement this based on how your Attack class is defined
        //attack.TimeBetweenShots -= chargeUpTime;
    }

}