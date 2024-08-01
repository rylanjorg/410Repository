using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;


[System.Serializable]
public class CooldownEvent : UnityEvent<float> { }


[System.Serializable] 
[InlineProperty]
public class SimpleCooldown 
{
    [TabGroup("tab1", "Inscribed", TextColor = "green")]  [SerializeField] public float cooldownDuration = 1.0f;

    [Title("Visual Effect Events")]
    [TabGroup("tab1", "Inscribed")] [SerializeField] [ListDrawerSettings(ShowFoldout = true, DraggableItems = false, ShowItemCount = false)] public List<CooldownEventTrigger> cooldownEventTriggers;

    public List<CooldownEventTrigger> CooldownEventTriggers
    {
        get { return cooldownEventTriggers; }
        set { cooldownEventTriggers = value; }
    }
}
