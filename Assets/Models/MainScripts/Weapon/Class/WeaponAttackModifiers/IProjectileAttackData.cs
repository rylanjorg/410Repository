using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectileAttackData 
{
    int CurrentAmmoCount { get; set; }
    Vector3 TargetDirection { get; set; }
    bool OverrideDirection { get; set; }
    Transform GunTip { get; set; }
    void Reload();
    void SpawnProjectiles(Vector3 direction, bool overrideDirection);
    void CreateProjectileSpawnPoints();
}
