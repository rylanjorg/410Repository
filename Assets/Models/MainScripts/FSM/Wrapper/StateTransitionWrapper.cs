using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

[Serializable]
public class StateTransitionsWrapper : IHasTransitions
{

    [SerializeField]
    [TabGroup("parentTab", "General", TextColor = "green")] private Transition[] transitions;
    [TabGroup("parentTab", "General", TextColor = "green")] public Transition[] Transitions => transitions;

}

