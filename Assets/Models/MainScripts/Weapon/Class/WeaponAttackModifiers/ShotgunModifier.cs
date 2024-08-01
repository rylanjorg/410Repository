using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using System;

public class ShotgunModifier : AttackModifier
{
    
    Weapon weaponInstance;
    [Title("Burst Fire Modifier", "This modifier will allow the weapon to fire multiple shots in a burst.")]
    [SerializeField] private float shotCount = 1.0f;
    [SerializeField] private float individualShotDelay = 1.0f;
    // The angle of the cone in degrees
    [SerializeField] float spreadRadius = 0.2f;
    [SerializeField] private List<int> overrideVFXIndices = new List<int>();
    

    public ShotgunModifier()
    {
        Type = ModifierType.ChargeUp;
    }

    public override void InitializeAttackModifier(Attack attack, Weapon weaponInstance)
    {
        //Debug.Log("Initializing Attack Modifier (ChargeUp)");
        base.InitializeAttackModifier(attack, weaponInstance);
    }

    public override bool ApplyModifier(bool ignoreModifiers, Action<AttackRuntimeData> baseAttack, AttackRuntimeData runtimeData)
    {
        if(runtimeData.attackInstance == null)
        {
            Debug.LogError("Apply modifier Attack is null");
            return false;
        }
    
        Debug.Log("BurstFireModifier");
        CoroutineStarter.Instance.StartCoroutine(PerformShotgunTypeAttack(baseAttack, runtimeData));
        return false;
    }


    private IEnumerator PerformShotgunTypeAttack(Action<AttackRuntimeData> baseAttack, AttackRuntimeData runtimeData)
    {
        IProjectileAttackData projectileAttack = runtimeData.TryCastProjectileAttackData();
        projectileAttack.OverrideDirection = true;

        for (int i = 0; i < shotCount; i++)
        {
            // Compute a random direction within a circular spread orthogonal to the custom forward vector
            float randomX = UnityEngine.Random.Range(-spreadRadius, spreadRadius);
            float randomZ = UnityEngine.Random.Range(-spreadRadius, spreadRadius);
            Vector3 spreadDirection = new Vector3(randomX, 0f, randomZ); // Ensure the spread is in the X-Z plane

            // Get a vector orthogonal to the custom forward vector
            Vector3 orthoVector = Vector3.Cross(projectileAttack.GunTip.forward.normalized, Vector3.up).normalized;

            // Transform the spread direction to be orthogonal to the custom forward vector
            Vector3 localSpreadDirection = spreadDirection.x * orthoVector + spreadDirection.z * Vector3.up;
            localSpreadDirection += projectileAttack.GunTip.TransformDirection(Vector3.forward);

            // Normalize the direction
            localSpreadDirection = localSpreadDirection.normalized;

            // Transform the local spread direction to world space
            //Vector3 worldSpreadDirection = projectileAttack.GunTip.TransformDirection(localSpreadDirection);

            // Set the target direction for the projectile attack
            projectileAttack.TargetDirection = localSpreadDirection;

            // Draw the vectors in the Scene view
            Debug.DrawRay(projectileAttack.GunTip.position, orthoVector, Color.red, 1.0f);
            Debug.DrawRay(projectileAttack.GunTip.position, localSpreadDirection, Color.green, 1.0f);
            //Debug.DrawRay(projectileAttack.GunTip.position, worldSpreadDirection, Color.blue, 1.0f);


            // Perform the attack
            if (baseAttack != null)
            {
                baseAttack(runtimeData);
                runtimeData.CallFunctionForMatchingValues(runtimeData.allVFXIDs, overrideVFXIndices);
            }
            else
            {
                Debug.LogError("baseAttack is null");
            }

            // Wait for the delay between shots
            yield return new WaitForSeconds(individualShotDelay);
        }

        projectileAttack.OverrideDirection = false;
    }



    public override void RemoveModifier(Attack attack)
    {
        // Decrease the time between shots
        // This is just an example, you'll need to implement this based on how your Attack class is defined
        //attack.TimeBetweenShots -= chargeUpTime;
    }

 

}