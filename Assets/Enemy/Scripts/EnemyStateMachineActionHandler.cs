using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Pathfinding;

[System.Serializable]
public class EnemyStateMachineActionHandler
{

    [BoxGroup("ProceduralAnim")] [SerializeField]  private ProceduralLegController legController;
    [BoxGroup("ProceduralAnim")] [SerializeField] GameObject PABodyTarget;
    [BoxGroup("ProceduralAnim")] [SerializeField] PATargetController paTargetController;
    [BoxGroup("ProceduralAnim")] [SerializeField] AIDestinationSetter aiDestinationSetter;
    [BoxGroup("ProceduralAnim")] [SerializeField] SecondOrderDemoPosition secondOrderDemoPosition;
    [BoxGroup("ProceduralAnim")] [SerializeField] float idlebodyHeight;
    [BoxGroup("ProceduralAnim")] [SerializeField] float movingbodyHeight;
    [BoxGroup("SimpleWeaponObject")] [SerializeField] SimpleWeaponObjectEnemy simpleWeaponObjectEnemy;

    [SerializeField] TurretEnemyStateMachine turretEnemyStateMachine;

    public void SetReferences(TurretEnemyStateMachine turretEnemyStateMachine)
    {
        this.turretEnemyStateMachine = turretEnemyStateMachine;
    }

    public void HandleIdleState()
    {

    }

    public void HandleRangeFollowState()
    {
        aiDestinationSetter.SetTarget(turretEnemyStateMachine.GetTarget());
        paTargetController.EnableAIPathFind();
        Vector3 position = PABodyTarget.transform.localPosition;
        position.y = movingbodyHeight;
        PABodyTarget.transform.localPosition = position;
    }

    public void HandleMeleeFollowState()
    {

    }

    public void HandleRangeAttackState()
    {
        paTargetController.DisableAIPathFind();
        simpleWeaponObjectEnemy.TryAttack();
    }

    public void HandleMeleeSwipeState()
    {

    }

    public void HandleMeleeChargeState()
    {

    }

    public void HandleDisabledState()
    {

    }

}
