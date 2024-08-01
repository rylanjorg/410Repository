using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackData
{
    public Attack attack; // Reference to the Attack ScriptableObject
    public AttackRuntimeData attackRuntimeData; // Dynamic cooldown data
    
    public AttackData(Attack attack, Weapon weapon)
    {
        this.attack = attack;
        this.attackRuntimeData = new AttackRuntimeData(attack, weapon);
    }
}