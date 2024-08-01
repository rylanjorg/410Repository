using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityHasWeapon
{
    WeaponObject WeaponObject { get; set; }
    GameObject WeaponInstance { get; set; }
    Weapon WeaponSettingsInstance { get; set; }
    Transform WeaponSpawnPoint { get; set; }
    Transform WeaponParentTransform { get; set; }
}
