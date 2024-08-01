using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;

public enum ModifierType
{
    // Define your modifier types here
    ChargeUp,
    // Add more as needed
}

[System.Serializable]
public abstract class AttackModifier 
{
    [LabelText("Modifier Type")] [HideInInspector] public ModifierType Type;

    public virtual void InitializeAttackModifier(Attack attackInstance, Weapon weaponInstance)
    {
        //AttackInstance = attackInstance;
        //WeaponInstance = weaponInstance;
    }

    // Method to apply the modifier to an attack
    public abstract bool ApplyModifier(bool ignoreModifiers, Action<AttackRuntimeData> baseAttack, AttackRuntimeData runtimeData);

    // Method to remove the modifier from an attack
    public abstract void RemoveModifier(Attack attack);

    public virtual AttackModifier Clone()
    {
        // Create a shallow copy of the current instance
        AttackModifier copy = (AttackModifier)this.MemberwiseClone();

        // Create a deep copy of the properties that need to be deep copied
        // For example, if you have a list that needs to be deep copied, you can do it like this:
        // copy.SomeList = new List<SomeType>(this.SomeList);

        // Return the copy
        return copy;
    }


}

    
