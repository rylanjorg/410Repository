using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;

[System.Serializable]
public class IdleState : State, IHasTransitions
{
    [SerializeField] [ListDrawerSettings(ShowIndexLabels = false)] public List<Transition> transitions;

    public Transition[] Transitions
    {
        get
        {
            return transitions.ToArray();
        }
    }

    public override State CreateInstance()
    {
        State stateInstance = Clone();
        return stateInstance;
    }


    public override void Enter()
    {
        // Enter logic for the idle state
    }

    public override void PerFrameUpdate()
    {
        // Update logic for the idle state
        //Debug.Log("Per Frame Update for IdleState");

        foreach (var transition in transitions)
        {
            foreach (var condition in transition.condition)
            {
            

                /*bool conditionMet = condition.useInvertedCondition == false ? condition.IsMet() : !condition.IsMet();
                //Debug.Log("Checking condition: " + condition + ". Is the condition met: " + conditionMet );

                if (conditionMet)
                {
                   // ChangeState(StateMachineReference.currentStateInstance, transition.targetState.state);
                    return;
                }*/
            }
            
        }
    }


    public override void Exit(State oldStateInstance)
    {
        // Exit logic for the idle state
    }

    public override State Clone()
    {
        string json = JsonUtility.ToJson(this);
        IdleState clone = JsonUtility.FromJson<IdleState>(json);
        return clone;
    }
}