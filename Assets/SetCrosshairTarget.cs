using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCrosshairTarget : MonoBehaviour
{
    public Transform gunTipTransform;
    void Awake()
    {
        GameObject.Find("PlayerRootContainer").GetComponent<AimCrosshair>().gunTipTransform = gunTipTransform;
    }
}
