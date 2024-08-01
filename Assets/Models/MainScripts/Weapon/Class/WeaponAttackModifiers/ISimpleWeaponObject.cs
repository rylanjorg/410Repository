using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

public interface ISimpleWeaponObject
{
    void TryAttack();
    int GetNextAvailiableAttackID();
    void SetReferences();
}
