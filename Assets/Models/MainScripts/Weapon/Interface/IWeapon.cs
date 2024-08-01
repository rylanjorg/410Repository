
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    public string[] DamageTagFilter { get; set; }

    [SerializeField]
    Transform RootTransform { get; set; }


    void PerformAttack();
}

/*public static class WeaponExtensions
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
}*/
