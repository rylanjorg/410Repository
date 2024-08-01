using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;



//[CreateAssetMenu(menuName = "Transitions/Transition")]
[System.Serializable]
public class Transition 
{
    [SerializeReference] [HideInInspector]
    public State targetStateInstance;
    [SerializeReference] [HideReferenceObjectPicker]
    public List<TransitionCondition> condition = new List<TransitionCondition>(){};
    public Transition targetState;

}