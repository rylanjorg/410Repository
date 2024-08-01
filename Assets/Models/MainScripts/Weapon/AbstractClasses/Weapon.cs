using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;

public abstract class Weapon 
{


    [Title("Base Weapon Class:")]
    [TabGroup("tab1", "Inscribed", TextColor = "green")]
    [TabGroup("tab1", "Inscribed")] [SerializeField] public GeneralWeaponData weaponData;
    [TabGroup("tab1", "Inscribed")] [SerializeField] public WeaponCreationHandler weaponCreationHandler = new WeaponCreationHandler();

    [Title("Weapon Attacks:")]
    [TabGroup("tab1", "Inscribed")] [SerializeField] protected List<AttackData> attacksData = new List<AttackData>();
    //[TabGroup("tab1", "Inscribed")] [SerializeField] [ListDrawerSettings(ShowFoldout = true, DraggableItems = false, ShowItemCount = false)] protected List<AttackModifierData> attackModifierData = new List<AttackModifierData>();

    [Title("Collision:")]
    [TabGroup("tab1", "Inscribed")]  [ValueDropdown("GetAllTags")] public string[] DamageTagFilter = new string[] { };
    [TabGroup("tab1", "Inscribed")]  [ValueDropdown("GetAllTags")] public string[] CollisionTagFilter = new string[] { };
    


    //[SerializeField] [TabGroup("tab1","VFXEvents", TextColor = "purple")] protected List<VisualEffectStruct> vfxStructs = new List<VisualEffectStruct>();




    [TabGroup("tab1", "Dynamic", TextColor = "blue")] [SerializeField] [ReadOnly] public GameObject weaponModelInstance;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] public float timeLastAttack = 0.0f;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] protected Quaternion initialRotation;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] protected Transform rootTransform;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] protected AudioSource audioSource; 
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] protected GameObject weaponRoot;
    [TabGroup("tab1", "Dynamic")] [SerializeField] [ReadOnly] protected Animator animator;
 

    


    public virtual string WeaponName
    {
        get { return weaponData.weaponName; }
        set { weaponData.weaponName = value; }
    }
    public virtual bool UseOutline
    {
        get { return weaponData.useOutline; }
        set { weaponData.useOutline = value; }
    }
    public virtual TransformData RootTransformData
    {
        get { return weaponData.rootTransformData; }
        set { weaponData.rootTransformData = value; }
    }

    public virtual Transform RootTransform
    {
        get { return rootTransform; }
        set { rootTransform = value; }
    }
    public virtual float AttackRange
    {
        get { return weaponData.attackRange; }
        set { weaponData.attackRange = value; }
    }

    public virtual GameObject WeaponModelPrefab
    {
        get { return weaponData.weaponModelPrefab; }
        set { weaponData.weaponModelPrefab = value; }
    }
    public virtual GameObject WeaponModelInstance
    {
        get { return weaponModelInstance; }
        set { weaponModelInstance = value; }
    }
    public virtual AudioSource MyAudioSource
    {
        get { return audioSource; }
        set { audioSource = value; }
    }
    public virtual float TimeLastAttack
    {
        get { return timeLastAttack; }
        set { timeLastAttack = value; }
    }
    public virtual Quaternion InitialRotation
    {
        get { return initialRotation; }
        protected set { initialRotation = value; }
    }

    public virtual List<AttackData> AttackData
    {
        get { return attacksData; }
        set { attacksData = value; }
    }

    public GameObject WeaponRoot
    {
        get { return weaponRoot; }
        set { weaponRoot = value; }
    }

    public RuntimeAnimatorController WeaponAnimation
    {
        get { return weaponData.weaponAnimation; }
        set { weaponData.weaponAnimation = value; }
    }

    public Animator Animator
    {
        get { return animator; }
        set { animator = value; }
    }


    public void ResetAttackTime()
    {
        timeLastAttack = 0.0f;
    }


    public void UpdateAttackTime()
    {
        timeLastAttack += Time.deltaTime;
    }



    public virtual void Update()
    {
        
        foreach(AttackData data in AttackData)
        {
            data.attackRuntimeData.TryUpdateUIPopUp();


            if(data.attack != null && data.attackRuntimeData.cooldownData != null)
            {
                data.attack.UpdateVar();
                if(data.attackRuntimeData.cooldownData.canUseCooldown == false)
                {
                    data.attackRuntimeData.cooldownData.UpdateCooldown(data.attack.simpleCooldown.cooldownEventTriggers, data.attack.simpleCooldown.cooldownDuration);
                }
            }
            else
            {
                Debug.LogError("Attack or SimpleCooldown is null");
            }
        }
    }


    public virtual void TryPickAttackAtRandom()
    {
        Debug.Log($"Picking attack at random");
        int attackIndex = UnityEngine.Random.Range(0, AttackData.Count - 1);
        AttackData[attackIndex].attack.PerformTypeAttack(AttackData[attackIndex].attackRuntimeData, false);
    }

    public virtual void TryPickAttack(Attack attack)
    {
        //attack.PerformTypeAttack(AttackData[attackIndex].attackRuntimeData, false);
    }

    public virtual void Reload()
    {
       foreach(AttackData data in AttackData)
       {
           if(data.attackRuntimeData.attackData is IProjectileAttackData projectileAttackData)
           {
               Debug.LogError("Reloading projectile attack");
               projectileAttackData.Reload();
           }
           
       }
    }

    public virtual void SpawnWeaponModel(Transform spawnLocationTransform, Transform parentTransform, ISimpleWeaponObject simpleWeaponObject)
    {
        GameObject weapon = weaponCreationHandler.CreateWeapon(this, spawnLocationTransform, parentTransform, simpleWeaponObject);
    }
    
    public abstract void PerformAttack(Weapon weaponInstance, int attackIndex);

    public Weapon ShallowCopy()
    {
        return (Weapon)this.MemberwiseClone();
    }

    public virtual Weapon Clone(Action<GameObject> weaponModelCallback)
    {
        WeaponCloner cloner = new WeaponCloner();
        Weapon clone = cloner.CloneWeapon(this, weaponModelCallback);
        return clone;
    }

    public static IEnumerable<string> GetAllTags()
    {
        return UnityEditorInternal.InternalEditorUtility.tags;
    }
}

public static class WeaponExtensions
{
    public static bool ArrayContainsTag(this IWeapon weapon, string tag)
    {
        foreach (string arrayTag in weapon.DamageTagFilter)
        {
            if (tag == arrayTag)
            {
                return true;
            }
        }
        return false;
    }

    public static Transform FindTransformInHierarchy(Transform parent, string name)
    {
        // Check if the current transform's name matches the target name
        if (parent.name == name)
        {
            return parent;
        }

        // Check all children
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform result = FindTransformInHierarchy(parent.GetChild(i), name);

            if (result != null)
            {
                return result;
            }
        }

        // If the transform was not found in this hierarchy, return null
        return null;
    }
}

