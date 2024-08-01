using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

[System.Serializable]
public class EnemyWeaponStateMachineActionHandler
{
    
    
    [TabGroup("StateHandler", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] public SimpleWeapon simpleWeapon;
    [TabGroup("StateHandler", "Dynamic")] [SerializeField] [ReadOnly] public int chosenAttackIndex;
    [TabGroup("StateHandler", "Dynamic")] [SerializeField] [ReadOnly] public SimpleWeaponObjectEnemy.AttackMode attackMode;

    public EnemyWeaponStateMachineActionHandler(SimpleWeapon simpleWeapon,  SimpleWeaponObjectEnemy.AttackMode attackMode =  SimpleWeaponObjectEnemy.AttackMode.Random)
    {
        this.simpleWeapon = simpleWeapon;
        this.attackMode = attackMode;
    }

    public void HandleCooldownState()
    {

    }

    public void HandleCanPerfromAttackState()
    {
        //simpleWeapon.PerformAttack(simpleWeapon,0);
    }

    public void HandlePerformingAttackState()
    {

    }

    public void HandleChoosingAttackState()
    {
        //Debug.LogError("Really trying to perform attack im really trying");

        int thisChosenAttackIndex = 0;

        switch(attackMode)
        {
            case  SimpleWeaponObjectEnemy.AttackMode.Random:
                thisChosenAttackIndex = Random.Range(0, simpleWeapon.AttackData.Count);
                break;
            case  SimpleWeaponObjectEnemy.AttackMode.Indexed:
                thisChosenAttackIndex = chosenAttackIndex;
                break;
        }

       
        // Perform the Weapon Attack type override at the attack Index
        if(simpleWeapon.AttackData[thisChosenAttackIndex].attackRuntimeData.cooldownData.canUseCooldown == true)
        {
            //simpleWeapon.PerformAttack(simpleWeapon, thisChosenAttackIndex);
            //simpleWeapon.TryPickAttack(chosenAttackInstance);
            //Debug.LogError($"Picking attack at index {thisChosenAttackIndex}");
            simpleWeapon.AttackData[thisChosenAttackIndex].attack.PerformTypeAttack(simpleWeapon.AttackData[thisChosenAttackIndex].attackRuntimeData, false);
            
        }
    }   

    public void HandleReloadingState()
    {
    }

    public void HandleDisabledState()
    {
    }
}