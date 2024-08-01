using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlignRotation : MonoBehaviour
{
    public float velocityInfluence = 0.1f;
    Vector3 forwardVector;
    Quaternion targetRotation;
    private void Awake()
    {
        forwardVector = PlayerInfo.Instance.playerRuntimeDataList[0].playerData.playerForwardVector;
        targetRotation = Quaternion.LookRotation(forwardVector);
    }
    private void Update()
    {
        this.transform.rotation = targetRotation;
    }
}