using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;


public class TurretEnemyStateMachine : MonoBehaviour
{
    public enum EnemyState 
    {
        Idle,
        RangeFollow,
        MeleeFollow,
        RangeAttack,
        MeleeSwipe,   // State for melee swipe attack
        MeleeCharge,  // State for melee charge attack
        Disabled
    }

    public EnemyStateMachineTransitionHandler enemyStateMachineTransitionHandler = new EnemyStateMachineTransitionHandler();
    public EnemyStateMachineActionHandler enemyStateMachineActionHandler = new EnemyStateMachineActionHandler();

    [TabGroup("SimpleWeaponObject", "General", TextColor = "green")] public Transform characterRoot;
    [TabGroup("SimpleWeaponObject", "General")] public LayerMask enemyTargetLayerMask;


    [TabGroup("SimpleWeaponObject", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] public EnemyState enemyState;





    public void Start()
    {   
        enemyStateMachineTransitionHandler.SetReferences(this);
        enemyStateMachineActionHandler.SetReferences(this);
        enemyStateMachineTransitionHandler.OnStart();
        enemyState = EnemyState.Idle;
    }


    public void Awake()
    {
        //AttackIDCounter = 0;
        //enemyStateMachineTransitionHandler 
        //enemyStateMachineActionHandler = new EnemyStateMachineActionHandler();
    }

    public void TryAttack()
    {
        //enemyStateMachineTransitionHandler.tryPerformAttack = true;
        //enemyStateMachineTransitionHandler.chosenAttackInstance = simpleWeapon.AttackData[Mathf.Clamp(0, 0, simpleWeapon.AttackData.Count - 1)].attack;
    }

    public Transform GetTarget()
    {
        // TODO: get relevant target for multiplayer
        return PlayerInfo.Instance.playerRuntimeDataList[0].generalData.playerRoot.transform;
    }


    public void Update()
    {
        enemyStateMachineTransitionHandler.OnUpdate();
        /*if()
        {
            SetReferences();
        }

        SetReferences();*/

        switch (enemyState)
        {
            case EnemyState.Idle:
                enemyStateMachineTransitionHandler.HandleIdleTransitions();
                enemyStateMachineActionHandler.HandleIdleState();
                break;
            case EnemyState.RangeFollow:
                enemyStateMachineTransitionHandler.HandleRangeFollowTransitions();
                enemyStateMachineActionHandler.HandleRangeFollowState();
                break;
            case EnemyState.MeleeFollow:
                enemyStateMachineTransitionHandler.HandleMeleeFollowTransitions();
                enemyStateMachineActionHandler.HandleMeleeFollowState();
                break;
            case EnemyState.RangeAttack:
                enemyStateMachineTransitionHandler.HandleRangeAttackTransitions();
                enemyStateMachineActionHandler.HandleRangeAttackState();
                break;
            case EnemyState.MeleeSwipe:
                enemyStateMachineTransitionHandler.HandleMeleeSwipeTransitions();
                enemyStateMachineActionHandler.HandleMeleeSwipeState();
                break;
            case EnemyState.MeleeCharge:
                enemyStateMachineTransitionHandler.HandleMeleeChargeTransitions();
                enemyStateMachineActionHandler.HandleMeleeChargeState();
                break;
            case EnemyState.Disabled:
                enemyStateMachineTransitionHandler.HandleDisabledTransitions();
                enemyStateMachineActionHandler.HandleDisabledState();
                break;
            default:
                break;
        }
    }

    public void SetReferences()
    {
        //enemyStateMachineTransitionHandler.simpleWeaponObject = this;
        //enemyStateMachineTransitionHandler.simpleWeapon = simpleWeapon;
        //enemyStateMachineActionHandler.simpleWeapon = simpleWeapon;
    }
}