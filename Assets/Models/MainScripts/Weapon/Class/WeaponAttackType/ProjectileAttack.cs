using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;

[System.Serializable]
[CreateAssetMenu(fileName = "New ProjectileAttack", menuName = "ProjectileAttack")]
public class ProjectileAttack : Attack, IProjectileAttack
{
    [SerializeField] [TabGroup("tab7","AttackType", TextColor = "orange")]  public int ammoCount;
    [SerializeField] [TabGroup("tab7","AttackType", TextColor = "orange")]  private AudioClip ammoOutSound;
    [SerializeField] [TabGroup("tab7","AttackType", TextColor = "orange")]  private float audioClipPitch;
    [SerializeField] [TabGroup("tab7","AttackType", TextColor = "orange")]  private float audioClipVolume;
    [SerializeField] [TabGroup("tab7","AttackType", TextColor = "orange")]  private float projectileSpeed;
    [SerializeField] [TabGroup("tab7","AttackType", TextColor = "orange")]  public GameObject projectilePrefab;
    [SerializeField] [TabGroup("tab7","AttackType", TextColor = "orange")]  private List<TransformData> projectileSpawnPoints;
    
    public AudioClip AmmoOutSound
    {
        get { return ammoOutSound; }
        set { ammoOutSound = value; }
    }

    public int AmmoCount
    {
        get { return ammoCount; }
        set { ammoCount = value; }
    }

    public float ProjectileSpeed
    {
        get { return projectileSpeed; }
        set { projectileSpeed = value; }
    }


    public GameObject ProjectilePrefab
    {
        get { return projectilePrefab; }
        set { projectilePrefab = value; }
    }
    public List<TransformData> ProjectileSpawnPoints
    {
        get { return projectileSpawnPoints; }
        set { projectileSpawnPoints = value; }
    }


    public override AttackType Type => AttackType.Projectile;

    protected override void BaseAttack(AttackRuntimeData attackRuntimeData)
    {
        base.BaseAttack(attackRuntimeData);
        //attackRuntimeData.weaponInstance.ResetAttackTime();
        //Debug.LogError("ProjectileAttack BaseAttack!");
        IProjectileAttackData projectileAttack = attackRuntimeData.TryCastProjectileAttackData();
        if(projectileAttack != null) projectileAttack.SpawnProjectiles(projectileAttack.TargetDirection, projectileAttack.OverrideDirection);
        else Debug.LogError("ProjectileAttackData is null");
    }

    public override void OnAwake(AttackRuntimeData attackRuntimeData)
    {
        base.OnAwake(attackRuntimeData);
        
        IProjectileAttackData projectileAttack = attackRuntimeData.TryCastProjectileAttackData();
        if(projectileAttack != null) projectileAttack.CreateProjectileSpawnPoints();
        else Debug.LogError("ProjectileAttackData is null");

        baseAttackDelegate = BaseAttack;
    }

    public override void PerformTypeAttack(AttackRuntimeData attackRuntimeData, bool ignoreModifiers, bool overrideCooldown = false)
    {
        Debug.Log("Performing type attack!" + attackRuntimeData.attackInstance + " ignore modifiers: " + ignoreModifiers + " override cooldown: " + overrideCooldown);
        
        if(!attackRuntimeData.cooldownData.canUseCooldown && !overrideCooldown)
        {
            Debug.Log("Attack is on cooldown");
            return;
        }

  

        if(attackRuntimeData.attackData is IProjectileAttackData projectileAttackData)
        {
            if(projectileAttackData.CurrentAmmoCount <= 0)
            {
                Debug.Log("No ammo left");
                PlayAmmoOutSound(attackRuntimeData.weaponInstance, attackRuntimeData.attackInstance);
                return;
            }
            else
            {
                Debug.Log("Ammo left: " + projectileAttackData.CurrentAmmoCount);
            }           
        }


        bool overrideBaseAttack = false;


        if(!ignoreModifiers)
        {
            // Check if Modifiers is not null
            if (attackRuntimeData.attackInstance.Modifiers != null)
            {
                // Apply all modifiers
                foreach (var modifier in attackRuntimeData.attackInstance.Modifiers)
                {
                    overrideBaseAttack = modifier.ApplyModifier(ignoreModifiers, baseAttackDelegate, attackRuntimeData);
                    if (overrideBaseAttack)
                    {
                        break;
                    }
                }
            }
        }

        //If no modifiers were applied, or none of them overrode the base function call, perform the base attack
        if (!overrideBaseAttack)
        {
            Debug.Log("Performing base attack" + overrideBaseAttack);
            BaseAttack(attackRuntimeData);  
        }
        
    }

    private void PlayAmmoOutSound(Weapon weaponInstance, Attack attackInstance)
    {
        //Play the audio clip
        if(ammoOutSound != null)
        {
            if( weaponInstance.MyAudioSource != null)
            {
                weaponInstance.MyAudioSource.pitch = audioClipPitch;
                weaponInstance.MyAudioSource.clip = ammoOutSound;
                weaponInstance.MyAudioSource.volume = audioClipVolume;

                // Set the position of the audio source
                weaponInstance.MyAudioSource.transform.position = weaponInstance.RootTransform.position;

                // Play the audio
                weaponInstance.MyAudioSource.Play();
            }
            else
            {
                Debug.LogError("Audio source is null");
            
            }
        }
    }

    public override Attack Clone()
    {
        // Create a shallow copy of the current instance
        ProjectileAttack copy = (ProjectileAttack)this.MemberwiseClone();

        // Create a deep copy of the Modifiers list
        copy.Modifiers = new List<AttackModifier>();
        foreach (AttackModifier modifier in this.Modifiers)
        {
            copy.Modifiers.Add(modifier.Clone()); // Assume that AttackModifier has a Clone method
        }

        // If SimpleCooldown is not null, create a new instance with the same properties
        /*if (this.SimpleCooldown != null)
        {
            copy.SimpleCooldown = this.SimpleCooldown.Clone();
        }*/

        // If IProjectileWeapon is implemented, create the projectile spawn points
        if (copy is IProjectileAttack projectileAttack)
        {
            projectileAttack.ProjectileSpawnPoints = new List<TransformData>();
            foreach(var spawnPoint in this.ProjectileSpawnPoints)
            {
                projectileAttack.ProjectileSpawnPoints.Add(spawnPoint.Clone());
            }
        }

        Debug.Log("Attack Clone" + copy.Modifiers.Count);
        return copy;
    }
}
