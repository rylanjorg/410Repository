using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;


[System.Serializable]
public class TimeElapsedCondition : TransitionCondition
{
    [TabGroup("tab1", "TimeElapsedCondition", TextColor = "purple")]
    [TabGroup("tab1/TimeElapsedCondition/SubTabGroup", "General", TextColor = "green")] public float duration;
    [TabGroup("tab1/TimeElapsedCondition/SubTabGroup", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] private float elapsedTime;
    
    public override bool IsMet()
    {

        elapsedTime += Time.deltaTime;
        bool isMet = elapsedTime >= duration;
        if(isMet)
        {
            elapsedTime = 0;
        }
        return isMet;
    }

    public override void Reset()
    {
        elapsedTime = 0;
    }


}
