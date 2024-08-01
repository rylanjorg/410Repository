using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXController : MonoBehaviour
{
    public VisualEffect visualEffect;
    public Vector3 myVector;


    
    void Awake()
    {
        myVector = -PlayerInfo.Instance.playerRuntimeDataList[0].playerData.playerForwardVector;
        Quaternion rotation = Quaternion.LookRotation(myVector);
        visualEffect.SetVector3("PlayerForwardVector", rotation.eulerAngles);
    }
}