using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;
using PlayerStates;

public class KillVFXOnPlayerState : MonoBehaviour
{
    //public Transform transformToAlign;
    //public Transform targetTransform;
    public FSMPlayer playerFSM;
    public MovementState targetState;
    public bool useContraryComparison = false;
    //public float velocityInfluence = 0.1f;

    private void Awake()
    {

        // Set targetTransform to be the parent of the current GameObject
        playerFSM = GameObject.Find("PlayerMovementManager").GetComponent<FSMPlayer>();
    }
    
    private void Update()
    {
        if(playerFSM != null)
        {
            if(playerFSM.playerData.playerRuntimeData.currentState == targetState && !useContraryComparison)
            {
                Destroy(gameObject);
            }
            else if(playerFSM.playerData.playerRuntimeData.currentState != targetState && useContraryComparison)
            {
                Destroy(gameObject);
            }
        }
    }
}
