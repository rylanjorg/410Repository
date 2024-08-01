using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyStateMachineTransitionHandler
{
    [SerializeField] private DistanceBetweenTwoPoints distanceBetweenTwoPoints;
    [SerializeField] private DistanceBetweenTwoPoints distanceBetweenTwoPoints_RangeFollow;
    [SerializeField] private DistanceBetweenTwoPoints distanceBetweenTwoPoints_RangeAttack;
    [SerializeField] private LineOfSightCondition lineOfSightCondition;
    [SerializeField] TurretEnemyStateMachine turretEnemyStateMachine;


    public void SetReferences(TurretEnemyStateMachine turretEnemyStateMachine)
    {
        this.turretEnemyStateMachine = turretEnemyStateMachine;

        lineOfSightCondition.SetReferences(turretEnemyStateMachine.characterRoot, turretEnemyStateMachine.enemyTargetLayerMask);
        distanceBetweenTwoPoints.SetReferences(turretEnemyStateMachine.characterRoot);
        distanceBetweenTwoPoints_RangeAttack.SetReferences(turretEnemyStateMachine.characterRoot);
        distanceBetweenTwoPoints_RangeFollow.SetReferences(turretEnemyStateMachine.characterRoot);
    }

    public void OnStart()
    {
        lineOfSightCondition.OnStart();
        distanceBetweenTwoPoints.OnStart();
        distanceBetweenTwoPoints_RangeAttack.OnStart();
        distanceBetweenTwoPoints_RangeFollow.OnStart();
    }

    public void OnUpdate()
    {

    }

    public void HandleIdleTransitions()
    {
        Debug.Log("HandleIdleTransitions: distanceBetweenTwoPoints.IsMet() = " + distanceBetweenTwoPoints.IsMet() + ", lineOfSightCondition.IsMet() = " + lineOfSightCondition.IsMet());
        if(distanceBetweenTwoPoints.IsMet()  && lineOfSightCondition.IsMet())
        {
            turretEnemyStateMachine.enemyState = TurretEnemyStateMachine.EnemyState.RangeFollow;
            return;
        }
    }

    public void HandleRangeFollowTransitions()
    {
        if(lineOfSightCondition.IsMet() && distanceBetweenTwoPoints_RangeFollow.IsMet())
        {
            turretEnemyStateMachine.enemyState = TurretEnemyStateMachine.EnemyState.RangeAttack;
            return;
        }
    }

    public void HandleMeleeFollowTransitions()
    {

    }

    public void HandleRangeAttackTransitions()
    {
        if(!distanceBetweenTwoPoints_RangeAttack.IsMet())
        {
            turretEnemyStateMachine.enemyState = TurretEnemyStateMachine.EnemyState.RangeFollow;
            return;
        }
    }

    public void HandleMeleeSwipeTransitions()
    {

    }

    public void HandleMeleeChargeTransitions()
    {

    }

    public void HandleDisabledTransitions()
    {

    }

}
