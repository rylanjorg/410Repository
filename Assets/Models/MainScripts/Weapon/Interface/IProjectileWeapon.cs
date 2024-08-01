using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileWeapon 
{
    GameObject ProjectilePrefab { get; set; }
    List<TransformData> ProjectileSpawnPoints { get; set; }
    List<Transform> ProjectileSpawnPointsInstance { get; set; }
    float ProjectileSpeed { get; set; }
    AudioClip ProjectileFireSound { get; set; } // Updated property name

}
