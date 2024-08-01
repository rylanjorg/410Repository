using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileAttack 
{
    float ProjectileSpeed { get; set; }
    int AmmoCount { get; set; }
    AudioClip AmmoOutSound { get; set; }
    GameObject ProjectilePrefab { get; set; }
    List<TransformData> ProjectileSpawnPoints { get; set; }
}
