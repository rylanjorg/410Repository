using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
  

public enum TargetMode
{
    PlayerOnly,
    LastDamageSource,
}

public class StateMachine : MonoBehaviour
{   
    [TabGroup("tab1","Inscribed", TextColor = "green")]
    [TabGroup("tab1/Inscribed/SubTabGroup", "General", TextColor = "lightgreen")] [SerializeField] private CharacterController characterController;
    [TabGroup("tab1/Inscribed/SubTabGroup", "General")] [SerializeField] private GameObject characterMovementRoot;
    
    [Title("Targeting:")]
    [TabGroup("tab1/Inscribed/SubTabGroup", "General")] [SerializeField] public GameObject characterTargetRoot;
    [TabGroup("tab1/Inscribed/SubTabGroup", "General")] [SerializeField] public TargetMode targetMode;

    [Title("References:")]
    [TabGroup("tab1/Inscribed/SubTabGroup", "General")] [SerializeField] public ProceduralLegController proceduralLegController;


    [TabGroup("tab1/Inscribed/SubTabGroup", "States", TextColor = "purple")] [SerializeReference] [ListDrawerSettings(ShowPaging = true)] public List<State> states = new List<State>{  };
    [TabGroup("tab1/Inscribed/SubTabGroup", "States")] [SerializeReference]  public State initialState;
    
    
   
    [TabGroup("tab1", "Dynamic", TextColor = "blue")] [ReadOnly] [SerializeReference] public State currentStateInstance;
    [TabGroup("tab1", "Dynamic")] [ReadOnly] public string currentStateName;
    [TabGroup("tab1", "Dynamic")] [ReadOnly] [SerializeField] private GameObject currentTarget { get; }
    [TabGroup("tab1", "Dynamic")] [ReadOnly] [SerializeField] public State initialStateInstance;
    [TabGroup("tab1", "Dynamic")] [SerializeReference] public List<State> stateInstances = new List<State> {};
    [TabGroup("tab1", "Dynamic")] [ReadOnly] [ShowIf("targetMode", TargetMode.PlayerOnly)] [SerializeField] public GameObject playerTargetRoot;
    

    void Awake()
    {
        if(playerTargetRoot == null)
        {
            GameObject[] playerRootRef = GameObject.FindGameObjectsWithTag("PlayerTargetRoot");
            if(playerRootRef.Length > 0)
            {
                Debug.Log("Found player target root: " + playerRootRef[0].name + ".");
                playerTargetRoot = playerRootRef[0];
            }
            else
            {
                Debug.LogError("No player target root found!");
            }
        }
    }

    void Start()
    {
        /*foreach (var stateWrapper in stateWrappers)
        {
            //State stateInstance = stateWrapper.state.CreateInstance();
           // stateInstance.StateMachineReference = this;


            
            // Use a callback to assign WeaponSettingsInstance after the weapon has been created
            TransitionConditionUtility.FindComponentWeaponModelAsync(this.gameObject, weapon =>
            {
                stateInstance.WeaponSettingsInstance = weapon.weaponSettingsInstance;
            });
            
            //stateInstances.Add(stateInstance);
        }*/

        foreach (var state in stateInstances)
        {
            if(state is IHasTransitions hasTransition)
            {

                foreach (var transition in hasTransition.Transitions)
                {
                    //State transitionState_Instance = stateInstances.Find(s => s.StateName == transition.targetState.state.StateName);
                    /*transition.targetStateInstance = transitionState_Instance;
                    
                    foreach (var condition in transition.condition)
                    {
                        if(condition is PAStateCondition paCondition)
                        {
                            paCondition.proceduralLegController = proceduralLegController;
                        }
                    }*/
                }
            }
            
        }

        // Find the state in stateInstances that matches the name of initialState
        State initialState_Instance = stateInstances.Find(s => s.StateName == initialState.StateName);

        if (initialState_Instance != null)
        {
            currentStateInstance = initialState_Instance;
            currentStateName = currentStateInstance.StateName;
            initialStateInstance = initialState_Instance;
        }
        else
        {
            // Handle the case where initialState is not found
            Debug.LogError("Initial state not found in stateInstances.");
        }
    }


    void Initialize()
    {
     
    }



    void Update()
    {
        /*(if (currentStateInstance != null && currentStateInstance is IHasTransitions stateWithTransitions)
        {
            foreach (var transition in stateWithTransitions.Transitions)
            {
                bool isTransitionValid = false;

                foreach (var condition in transition.condition)
                {
                    condition.OnEnter(this);
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
                    currentStateInstance.ChangeState(currentStateInstance, transition.targetStateInstance);
                    currentStateInstance = transition.targetStateInstance;
                    currentStateName = currentStateInstance.StateName;
                    return;
                }
            }
        }

        if (currentStateInstance != null)
            currentStateInstance.PerFrameUpdate();*/
    }

    void OnDrawGizmos()
    {

        if (currentStateInstance != null && currentStateInstance is IHasTransitions stateWithTransitions)
        {
            // Check transitions
            foreach (var transition in stateWithTransitions.Transitions)
            {
                foreach (var condition in transition.condition)
                {
                    condition.DrawGizmos();
                }
            }
        }
    }
}

