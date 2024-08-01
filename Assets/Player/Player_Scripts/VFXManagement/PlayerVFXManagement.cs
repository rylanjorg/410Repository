using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

using PlayerData;
using PlayerStates;

public class PlayerVFXManagement : MonoBehaviour
{
    private FSMPlayer fsmPlayer;
    public GameObject mainCamera;
    public GameObject speedLinesVFX;
    public Vector3 speedLinesVFXOffset;
    private GameObject speedLinesVFXInstance;

    void Awake()
    {
        fsmPlayer = FindObjectOfType<FSMPlayer>();
        if(fsmPlayer == null) Debug.LogError("FSMMovement not found");
    }

    // Update is called once per frame
    void Update()
    {
        if(fsmPlayer.playerData.playerRuntimeData.currentState == MovementState.Sliding && speedLinesVFXInstance == null)
        {
            //Debug.Log("Speed Lines VFX");
            speedLinesVFXInstance = Instantiate(speedLinesVFX, mainCamera.transform.position, Quaternion.identity);    
            speedLinesVFXInstance.transform.SetParent(mainCamera.transform);

            // Set local position and rotation
            speedLinesVFXInstance.transform.localPosition = speedLinesVFXOffset;
            speedLinesVFXInstance.transform.localRotation = Quaternion.identity; // Adjust this if needed


            speedLinesVFXInstance.GetComponent<VisualEffect>().Play();
        } 
        else if(speedLinesVFXInstance != null && fsmPlayer.playerData.playerRuntimeData.currentState != MovementState.Sliding)
        {
            //Debug.Log("Destroy Speed Lines VFX");
            Destroy(speedLinesVFXInstance);
        }
    }
}
