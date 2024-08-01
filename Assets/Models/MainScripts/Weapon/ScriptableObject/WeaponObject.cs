using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/WeaponObject")]
[System.Serializable]
public class WeaponObject : ScriptableObject
{
    [SerializeReference]
    public Weapon weapon;
}
    

    

