using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackData : AbstractAttackData, IProjectileAttackData
{
    private AttackRuntimeData attackRuntimeData;
    [SerializeField] private Transform gunTip;
    [SerializeField] public Vector3 targetDirection;
    [SerializeField] public bool overrideDirection;
    public int currentAmmoCount;

    public int CurrentAmmoCount
    {
        get { return currentAmmoCount; }
        set { currentAmmoCount = value; }
    }

    public Vector3 TargetDirection
    {
        get { return targetDirection; }
        set { targetDirection = value; }
    }

    public bool OverrideDirection
    {
        get { return overrideDirection; }
        set { overrideDirection = value; }
    }

    public Transform GunTip
    {
        get { return gunTip; }
        set { gunTip = value; }
    }



    private Attack attack;
    [SerializeField] private  List<Transform> projectileSpawnsGO;

    public ProjectileAttackData(AttackRuntimeData attackRuntimeData, Attack attack) 
    {
        this.gunTip = WeaponExtensions.FindTransformInHierarchy(attackRuntimeData.weaponInstance.weaponModelInstance.transform, "GunTip");            
        this.attackRuntimeData = attackRuntimeData;
        this.attack = attack;
        this.targetDirection = Vector3.zero;
        this.overrideDirection = false;

        if (attackRuntimeData.attackInstance is IProjectileAttack projectileAttack)
        {
            this.currentAmmoCount = projectileAttack.AmmoCount;
        }
    }

    public void Relaod()
    {
        if (attackRuntimeData.attackInstance is IProjectileAttack projectileAttack)
        {
            this.currentAmmoCount = projectileAttack.AmmoCount;
        }
    }
    
    public void SpawnProjectiles(Vector3 direction, bool overrideDirection)
    {
        // Check if the weaponInstance implements the IProjectileWeapon interface
        if (attackRuntimeData.attackInstance is IProjectileAttack projectileAttack)
        {
            // Access the ProjectileSpawnPoints from the interface
            foreach (var spawnPoint in projectileSpawnsGO)
            {
                if(projectileAttack.ProjectilePrefab != null)
                {
                    GameObject projectile = GameObject.Instantiate(projectileAttack.ProjectilePrefab, spawnPoint.position , spawnPoint.rotation);
                    var projectileRb = projectile.GetComponent<Rigidbody>();

                    if(overrideDirection)
                    {
                        projectileRb.velocity = direction * projectileAttack.ProjectileSpeed;
                    }
                    else
                    {
                        projectileRb.velocity = gunTip.TransformDirection(Vector3.forward) * projectileAttack.ProjectileSpeed;
                    }

                    currentAmmoCount--;
                }
                else
                {
                    Debug.LogError("ProjectilePrefab is null");
                }
            }
            attackRuntimeData.weaponInstance.TimeLastAttack = Time.time;
        }
        else
        {
            Debug.LogError("ProjectilePrefab is null 1");
        }
    }



    public void Reload()
    {
        if (attackRuntimeData.attackInstance is IProjectileAttack projectileAttack)
        {
            this.currentAmmoCount = projectileAttack.AmmoCount;
        } 
    }

    
    public void CreateProjectileSpawnPoints()
    {
        //Debug.LogError("CreateProjectileSpawnPoints");
        // Check if the weaponInstance implements the IProjectileWeapon interface
        if (attack is IProjectileAttack projectileAttack)
        {
            int counter = 0;
            projectileSpawnsGO = new List<Transform>();
            if(projectileAttack.ProjectileSpawnPoints.Count == 0)
            {
                Debug.LogError("ProjectileSpawnPoints is empty");
                return;
            }

            foreach (var spawnPoint in projectileAttack.ProjectileSpawnPoints)
            {
                GameObject projSpawnLocObj = SerializationUtility.CreateGameObjectFromTransformData(spawnPoint, "ProjectileSpawnPoint_" + counter, attackRuntimeData.weaponInstance.WeaponRoot.transform, 10);
                projectileSpawnsGO.Add(projSpawnLocObj.transform);
                counter++;
            }
        }
    }
    
   
}
