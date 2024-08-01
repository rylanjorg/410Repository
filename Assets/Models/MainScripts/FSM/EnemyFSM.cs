using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEditor.Experimental.GraphView;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;


public class EnemyFSM : MonoBehaviour
{
    //FSM state
    [Header("FSM state")]
    public EnemyState currentState;

    [Header("Enemy Inscribed")]
    public GameObject enemyRoot;
    private CharacterController enemyController;

    public enum EnemyState
    {
        Idle,
        Walking,
        Jumping,
        Falling, 
    }

    private void Awake()
    {
        enemyController = enemyRoot.GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Set the starting state
        currentState = EnemyState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (currentState)
        {
            case EnemyState.Idle:
                HandleIdleState();
                break;
            case EnemyState.Jumping:
                HandleJumpingState();
                break;
            case EnemyState.Falling:
                HandleFallingState();
                break;
            case EnemyState.Walking:
                HandleWalkingState();
                break;
        }
    }

    void HandleIdleState()
    {

    }

    void HandleJumpingState()
    {

    }

    void HandleFallingState()
    {

    }

    void HandleWalkingState()
    {

    }
}
