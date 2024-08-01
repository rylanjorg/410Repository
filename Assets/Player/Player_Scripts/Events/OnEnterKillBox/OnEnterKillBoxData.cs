using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

[CreateAssetMenu(menuName = "ScriptableObject/Event/OnEnterKillBoxData")]
public class OnEnterKillBoxData : ScriptableObject
{
    public List<VisualEffectStruct> onTeleportVFX = new List<VisualEffectStruct>();
    public List<VisualEffectStruct> onTeleportAnticipationVFX = new List<VisualEffectStruct>();
}