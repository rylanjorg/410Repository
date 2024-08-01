using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

[System.Serializable]
public class HitscanAttack : Attack, IHitscanAttack
{
 
    public override AttackType Type => AttackType.Hitscan;

    public override void OnAwake(AttackRuntimeData attackRuntimeData)
    {
        base.OnAwake(attackRuntimeData);
    }
}
