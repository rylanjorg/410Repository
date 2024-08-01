using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

[System.Serializable]
public class EnemyWeaponStateMachineTransitionHandler
{
    [TabGroup("TransitionHandler", "Inscribed")] [SerializeField] public KeyCode reloadKey = KeyCode.R;
    [TabGroup("TransitionHandler", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] public SimpleWeaponObjectEnemy simpleWeaponObject;
    [TabGroup("TransitionHandler", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] public SimpleWeapon simpleWeapon;
    [TabGroup("TransitionHandler", "Dynamic")] [SerializeField] [ReadOnly] public float timeSinceLastAttack;
    [TabGroup("TransitionHandler", "Dynamic")] [SerializeField] [ReadOnly] public bool tryPerformAttack;
    [TabGroup("TransitionHandler", "Dynamic")] [SerializeReference] [ReadOnly] public Attack chosenAttackInstance;
    [TabGroup("TransitionHandler", "Dynamic")] [SerializeReference] [ReadOnly] public int chosenAttackIndex;
    [TabGroup("TransitionHandler", "Dynamic")] [SerializeField] [ReadOnly] private bool cooldownActive;
    private Coroutine reloadCoroutine;

    public EnemyWeaponStateMachineTransitionHandler(SimpleWeaponObjectEnemy simpleWeaponObject)
    {
        tryPerformAttack = false;
        timeSinceLastAttack = 0.0f;
        chosenAttackInstance = null;
        this.simpleWeaponObject = simpleWeaponObject;
        simpleWeaponObject.weaponState = SimpleWeaponObjectEnemy.WeaponState.Cooldown;
        cooldownActive = true;
    }


    public void OnUpdate()
    {
        timeSinceLastAttack = simpleWeapon.TimeLastAttack;
        simpleWeapon.UpdateAttackTime();
        cooldownActive = timeSinceLastAttack <= simpleWeaponObject.attackInterval;
        //Debug.Log("Current State: " + simpleWeaponObject.weaponState + " attack interval: " + simpleWeapon.AttackInterval);
    }



    public void HandleCooldownTransitions()
    {
        if(!cooldownActive)
        {
            Debug.Log("Transitioning " + simpleWeaponObject.weaponState);
            simpleWeaponObject.weaponState = SimpleWeaponObjectEnemy.WeaponState.CanPerformAttack;
            return;
        }
    }

    public void HandleCanPerfromAttackTransitions()
    {
       // Debug.Log("Testing Can perform attack transition");

        if(Input.GetKeyDown(reloadKey) || Input.GetKey(reloadKey))
        {
            simpleWeaponObject.weaponState = SimpleWeaponObjectEnemy.WeaponState.Reloading;
            Debug.Log("Transitioning " + simpleWeaponObject.weaponState);
            return;
        }
        else 
        {
            if(tryPerformAttack) 
            {
                Debug.Log("Transitioning " + simpleWeaponObject.weaponState);
                simpleWeaponObject.weaponState = SimpleWeaponObjectEnemy.WeaponState.ChoosingAttack;
                tryPerformAttack = false;
                cooldownActive = true;
                return;
            }
        }

        
    }

    public void HandlePerformingAttackTransitions()
    {
        if(timeSinceLastAttack >= simpleWeaponObject.attackInterval && tryPerformAttack && simpleWeapon.AttackData[chosenAttackIndex].attackRuntimeData.cooldownData.canUseCooldown == true)
        {
            if(chosenAttackInstance != null && simpleWeapon.AttackData[chosenAttackIndex].attackRuntimeData.cooldownData != null && simpleWeapon.AttackData[chosenAttackIndex].attackRuntimeData.cooldownData.canUseCooldown == true)
            {
                simpleWeapon.ResetAttackTime();
            }
            
           // Debug.Log("Transitioning " + simpleWeaponObject.weaponState);
            simpleWeaponObject.weaponState = SimpleWeaponObjectEnemy.WeaponState.Cooldown;
            return;
        }
    }

    public void HandleChoosingAttackTransitions()
    {
        simpleWeaponObject.weaponState = SimpleWeaponObjectEnemy.WeaponState.PerformingAttack;
        //simpleWeapon.TimeLastAttack = Time.time;
        return;
       // if(chosenAttackIndex != -1 && tryPerformAttack)
       // {
            
            

            
           
       // }
    }   

    public void HandleReloadingTransitions()
    {
        // If the player dequips their weapon, stop the reload process
        /*if (weaponDequipped)
        {
            if (reloadCoroutine != null)
            {
                CoroutineStarter.Instance.StopCoroutine(reloadCoroutine);
                reloadCoroutine = null;
            }
            simpleWeaponObject.weaponState = SimpleWeaponObject.WeaponState.Idle; // Or whatever state represents the weapon being dequipped
        }*/
        // If the reload process is not already running, start it
        if (reloadCoroutine == null)
        {
            reloadCoroutine = CoroutineStarter.Instance.StartCoroutine(ReloadWeapon());
        }
    }

    public void HandleDisabledTransitions()
    {
    }

    private IEnumerator ReloadWeapon()
    {
        // Wait for the reload time to pass
        yield return new WaitForSeconds(simpleWeaponObject.reloadTime);

        // Reload the weapon
        simpleWeaponObject.simpleWeapon.Reload();

        // Transition to the next state
        simpleWeaponObject.weaponState = SimpleWeaponObjectEnemy.WeaponState.CanPerformAttack; // Or whatever state represents the weapon being ready to fire

        // Clear the reload coroutine
        reloadCoroutine = null;
    }
}
