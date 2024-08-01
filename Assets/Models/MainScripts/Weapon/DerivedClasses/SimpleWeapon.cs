using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Newtonsoft.Json;

[System.Serializable]
public class SimpleWeapon : Weapon
{
    [Button("Add Projectile Attack")]
    private void AddProjectileAttack()
    {
        //Attacks.Add(new ProjectileAttack());
    }

    [Button("Add Hitscan Attack")]
    private void AddHitscanAttack()
    {
        //Attacks.Add(new HitscanAttack());
    }


    public void AddAttack(Attack attack)
    {
        //Attacks.Add(attack);
    }

    public void RemoveAttack(Attack attack)
    {
        //Attacks.Remove(attack);
    }

    public override void PerformAttack(Weapon weaponInstance, int attackIndex)
    {
        if(AttackData == null)
        {
            Debug.LogError("Attacks List is null");
            return;
        }

        AttackData[attackIndex].attack.PerformTypeAttack(AttackData[attackIndex].attackRuntimeData, false);

        if(attackIndex < AttackData.Count)
        {
           
        }
        else
        {
            Debug.LogError("Attack index out of range");
        }
    }
}