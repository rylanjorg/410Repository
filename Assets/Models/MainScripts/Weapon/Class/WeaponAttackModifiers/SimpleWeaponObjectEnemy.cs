using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;



public class SimpleWeaponObjectEnemy : MonoBehaviour, ISimpleWeaponObject
{
    public enum WeaponState 
    {
        Cooldown,
        CanPerformAttack,
        ChoosingAttack,
        PerformingAttack,
        Reloading,
        Disabled
    }

    public enum AttackMode{
        Random,
        Indexed
    }
    
    public SimpleWeapon simpleWeapon = new SimpleWeapon(); 
    public EnemyWeaponStateMachineTransitionHandler weaponStateMachineTransitionHandler;
    public EnemyWeaponStateMachineActionHandler weaponStateMachineActionHandler;
    private int AttackIDCounter = 0;

    [TabGroup("SimpleWeaponObject", "Inscribed", TextColor = "green")] [SerializeField] private Transform weaponSpawnPoint;
    [TabGroup("SimpleWeaponObject", "Inscribed")] [SerializeField] private Transform weaponParentTransform;
    [TabGroup("SimpleWeaponObject", "Inscribed")] [SerializeField] public float attackInterval = 5.0f;
    [TabGroup("SimpleWeaponObject", "Inscribed")] [SerializeField] public float reloadTime = 5.0f;
    [TabGroup("SimpleWeaponObject", "Inscribed")] [SerializeField] public AttackMode attackMode;
    [TabGroup("SimpleWeaponObject", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] public WeaponState weaponState;

    public void Start()
    {
        if(simpleWeapon != null && weaponSpawnPoint != null && weaponParentTransform != null)
        {   
           simpleWeapon.SpawnWeaponModel(weaponSpawnPoint, weaponParentTransform, this);
        }
    
        weaponState = WeaponState.Cooldown;
    }


    public void Awake()
    {
        AttackIDCounter = 0;
        weaponStateMachineTransitionHandler = new EnemyWeaponStateMachineTransitionHandler(this);
        weaponStateMachineActionHandler = new EnemyWeaponStateMachineActionHandler(simpleWeapon, attackMode);
    }

    public void Update()
    {
        if(simpleWeapon != null && weaponSpawnPoint != null && weaponParentTransform != null && simpleWeapon.WeaponModelInstance == null)
        {   
            simpleWeapon.SpawnWeaponModel(weaponSpawnPoint, weaponParentTransform, this);
        }
        
        simpleWeapon.Update();
        weaponStateMachineTransitionHandler.OnUpdate();
    }

    public void TryAttack()
    {

        Debug.Log("Enemy Trying to attack");
        if(simpleWeapon.TimeLastAttack <= attackInterval)
        {
            weaponStateMachineTransitionHandler.tryPerformAttack = false;
            Debug.Log("Enemy Weapon is on cooldown");
            return;
        }
        weaponStateMachineTransitionHandler.tryPerformAttack = true;
        weaponStateMachineTransitionHandler.chosenAttackInstance = simpleWeapon.AttackData[Mathf.Clamp(0, 0, simpleWeapon.AttackData.Count - 1)].attack;
    }

    public int GetNextAvailiableAttackID()
    {
        int returnNumber = AttackIDCounter;
        AttackIDCounter++;
        return returnNumber;
    }

    public void FixedUpdate()
    {
        if(weaponStateMachineTransitionHandler.simpleWeapon == null || weaponStateMachineActionHandler.simpleWeapon == null || weaponStateMachineTransitionHandler.simpleWeaponObject == null)
        {
            SetReferences();
        }

        SetReferences();

        switch (weaponState)
        {
            case WeaponState.PerformingAttack:
                weaponStateMachineTransitionHandler.HandlePerformingAttackTransitions();
                weaponStateMachineActionHandler.HandlePerformingAttackState();
                break;
            case WeaponState.CanPerformAttack:
                weaponStateMachineTransitionHandler.HandleCanPerfromAttackTransitions();
                weaponStateMachineActionHandler.HandleCanPerfromAttackState();
                break;
            case WeaponState.ChoosingAttack:
                weaponStateMachineTransitionHandler.HandleChoosingAttackTransitions();
                weaponStateMachineActionHandler.HandleChoosingAttackState();
                break;
            case WeaponState.Cooldown:
                weaponStateMachineTransitionHandler.HandleCooldownTransitions();
                weaponStateMachineActionHandler.HandleCooldownState();
                break;
            case WeaponState.Reloading:
                weaponStateMachineTransitionHandler.HandleReloadingTransitions();
                weaponStateMachineActionHandler.HandleReloadingState();
                break;
            case WeaponState.Disabled:
                weaponStateMachineTransitionHandler.HandleDisabledTransitions();
                weaponStateMachineActionHandler.HandleDisabledState();
                break;
            default:
                break;
        }
    }

    public void SetReferences()
    {
        weaponStateMachineTransitionHandler.simpleWeaponObject = this;
        weaponStateMachineTransitionHandler.simpleWeapon = simpleWeapon;
        weaponStateMachineActionHandler.simpleWeapon = simpleWeapon;
    }
}
