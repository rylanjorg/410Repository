using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;


[System.Serializable]
public class PAStateCondition : TransitionCondition
{
    [TabGroup("tab1", "PAStateCondition", TextColor = "purple")]
    [TabGroup("tab1/PAStateCondition/SubTabGroup", "General", TextColor = "green")] public ProceduralLegController proceduralLegController;
    [TabGroup("tab1/PAStateCondition/SubTabGroup", "General")] [SerializeField] private ProceduralLegController.State isMetState;

    public override bool IsMet()
    {
        return proceduralLegController.currentState == isMetState;
    }

    
}
