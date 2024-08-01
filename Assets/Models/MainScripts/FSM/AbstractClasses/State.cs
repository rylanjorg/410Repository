using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;

[System.Serializable]
public abstract class State
{
    [TabGroup("parentTab", "General", TextColor = "green")] [SerializeField] protected string stateName = "DefaultState";

    // Property to get and set stateName
    public virtual string StateName
    {
        get { return stateName; }
        set { stateName = value; }
    }

    
   
    [TabGroup("parentTab", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] protected StateMachine stateMachineReference;
    public virtual StateMachine StateMachineReference
    {
        get { return stateMachineReference; }
        set { stateMachineReference = value; }
    }


    [SerializeReference]
    [TabGroup("parentTab", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] protected Weapon weaponSettingsInstance;
    public virtual Weapon WeaponSettingsInstance
    {
        get { return weaponSettingsInstance; }
        set { weaponSettingsInstance = value; }
    }

    

    public abstract State CreateInstance();
    public abstract void Enter();
    public abstract void PerFrameUpdate();
    public abstract void Exit(State stateInstance);
    public virtual State Clone()
    {
        string json = JsonUtility.ToJson(this);
        State clone = JsonUtility.FromJson<State>(json);
        return clone;
    }

    // Each state class can have its own way of handling state changes
    public virtual void ChangeState(State OldStateInstance, State newStateInstance)
    {
        // Implement transition logic here, if needed
        OldStateInstance.Exit(OldStateInstance);
        newStateInstance.Enter();
    }
}
