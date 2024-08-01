using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "ScriptableObjects/WeaponData", order = 1)]
public class GeneralWeaponData : ScriptableObject
{
    [SerializeField] public string weaponName = "DefaultName";
    [SerializeField] public bool useOutline = true;
    [SerializeField] public float attackRange = 1f;
    [SerializeField] public TransformData rootTransformData;
    [SerializeField] public GameObject weaponModelPrefab;
    [SerializeField] public bool addAnimator;
    [SerializeField] public RuntimeAnimatorController weaponAnimation;
}