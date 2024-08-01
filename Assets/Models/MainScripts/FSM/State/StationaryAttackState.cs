using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;

[System.Serializable]
public class StationaryAttackState : State, IHasTransitions
{
    [ListDrawerSettings(ShowIndexLabels = true)]
    [TabGroup("parentTab", "General")] public StateTransitionsWrapper transitionWrapper;
    [TabGroup("parentTab", "Dynamic")] [ReadOnly] public Quaternion exitRotationY;

    public Transition[] Transitions
    {
        get
        {
            // Combine transitions from all wrappers
            var combinedTransitions = new List<Transition>();
            combinedTransitions.AddRange(transitionWrapper.Transitions);
            return combinedTransitions.ToArray();
        }
    }

    public override State CreateInstance()
    {
        State stateInstance = Clone();
        return stateInstance;
    }

    public override void Enter()
    {
        Debug.Log("Entering StationaryAttackState");
        foreach (var transition in transitionWrapper.Transitions)
        {
            foreach (var condition in transition.condition)
            {       
                condition.Reset();
            }
        }
    }

    public override void PerFrameUpdate()
    {
    
        // Update logic for the idle state

        /*foreach (var transition in transitionWrapper.Transitions)
        {
            bool isTransitionValid = false;

            foreach (var condition in transition.condition)
            {
                bool conditionMet = condition.useInvertedCondition == false ? condition.IsMet() : !condition.IsMet();

                if (condition.conditionalType == TransitionCondition.ConditionType.And)
                {
                    if (!conditionMet)
                    {
                        isTransitionValid = false; 
                        Debug.Log($"And Condition {condition.GetType()} is not met");
                        break;
                    }
                    Debug.Log($"And Condition {condition.GetType()} is met");
                    isTransitionValid = true;
                }
                else if (condition.conditionalType == TransitionCondition.ConditionType.Or)
                {
                    if (conditionMet)
                    {
                        Debug.Log($"Or Condition {condition.GetType()} is met");
                        isTransitionValid = true;
                        break;
                    }
                    Debug.Log($"Or Condition {condition.GetType()} is not met");
                }
            }

            if (isTransitionValid)
            {
                if(StateMachineReference == null) Debug.LogError("StateMachineReference is null");
                ChangeState(StateMachineReference.currentStateInstance, transition.targetState.state);
                return;
            }
        }*/
    }



    public override void Exit(State oldStateInstance)
    {
        
        Debug.Log("Exiting StationaryAttackState");
        // Exit logic for the Follow state
        if (oldStateInstance is StationaryAttackState followStateInstance)
        {
            if (followStateInstance.weaponSettingsInstance != null)
            {
                followStateInstance.exitRotationY = Quaternion.Euler(0, followStateInstance.WeaponSettingsInstance.RootTransform.rotation.eulerAngles.y, 0);
            }
            else
            {
                Debug.LogError("weaponSettingsInstance is null");
            }
        }
    }


    public override State Clone()
    {
        string json = JsonUtility.ToJson(this);
        StationaryAttackState clone = JsonUtility.FromJson<StationaryAttackState>(json);
        return clone;
    }
}
