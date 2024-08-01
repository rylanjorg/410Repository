using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
using System;

[System.Serializable]
public class ProjectileWeapon : Weapon, IProjectileWeapon
{
    [SerializeField]
    [TabGroup("tab2","Inscribed IProjectile: General", TextColor = "green")]
    private float projectileSpeed;
    public float ProjectileSpeed
    {
        get => projectileSpeed;
        set => projectileSpeed = value;
    }
    
    [TabGroup("tab2","Inscribed IProjectile: Reference", TextColor = "orange")]
    //private string projectilePrefabAddress;
    [SerializeField]
    private GameObject projectilePrefab;
    public GameObject ProjectilePrefab
    {
        get => projectilePrefab;
        set => projectilePrefab = value;
    }


    

   
    [TabGroup("tab2","Inscribed IProjectile: Reference")]
    [SerializeField]
    private List<TransformData> projectileSpawnPoints;
    public List<TransformData> ProjectileSpawnPoints
    {
        get => projectileSpawnPoints;
        set => projectileSpawnPoints = value;
    }

    [TabGroup("tab2","Dynamic IProjectile:")]
    [SerializeField]
    private List<Transform> projectileSpawnPointsInstance;
    public List<Transform> ProjectileSpawnPointsInstance
    {
        get => projectileSpawnPointsInstance;
        set => projectileSpawnPointsInstance = value;
    }

    [SerializeField]
    [TabGroup("tab2","Inscribed IProjectile: Audio")]
    private AudioClip projectileFireSound;
    public AudioClip ProjectileFireSound
    {
        get => projectileFireSound;
        set => projectileFireSound = value;
    }

    public override void PerformAttack(Weapon weaponInstance, int attackIndex)
    {
        
        // Check if the weaponInstance implements the IProjectileWeapon interface
        if (weaponInstance is IProjectileWeapon projectileWeapon)
        {
            // Access the ProjectileSpawnPoints from the interface
            //Debug.Log($"ProjectileSpeed: {projectileWeapon.ProjectileSpeed}");
            foreach (var spawnPoint in projectileWeapon.ProjectileSpawnPointsInstance)
            {
                GameObject projectile = GameObject.Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
                var projectileRb = projectile.GetComponent<Rigidbody>();
                projectileRb.velocity = spawnPoint.TransformDirection(Vector3.forward) * projectileSpeed;

                if (MyAudioSource != null && projectileFireSound != null)
                    MyAudioSource.PlayOneShot(projectileFireSound);
            }

            timeLastAttack = Time.time;
        }
        else
        {
            // Handle the case where the weaponInstance doesn't implement IProjectileWeapon
            Debug.LogError("Weapon instance does not implement IProjectileWeapon");
        }
    }




    public override Weapon Clone(Action<GameObject> weaponModelCallback)
    {
        string json = JsonUtility.ToJson(this);
        ProjectileWeapon clone = JsonUtility.FromJson<ProjectileWeapon>(json);
        return clone;
    }
}
