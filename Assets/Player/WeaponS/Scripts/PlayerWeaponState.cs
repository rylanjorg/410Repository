using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWeaponState 
{
    public Transform parentTransform;
    [Range(0,1)] public float handIKAmount;
    [Range(0,1)] public float elbowIKAmount;
    [Range(0,1)] public float aimIKAmount;
}
