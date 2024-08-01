using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

public class WeaponHitBox : MonoBehaviour
{
    Weapon weaponInstanceReference;
    // Start is called before the first frame update
    void Start()
    {
        weaponInstanceReference = GetComponentInParent<WeaponModel>().weaponSettingsInstance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void OnTriggerEnter(Collider other)
    {
        Debug.LogError("OnTriggerEnter");
        if(weaponInstanceReference.DamageTagFilter.Contains(other.tag))
        {
            Debug.Log("Hit " + other.name);

            // Access the EnemyInfo script component on the enemy GameObject
            HitBox enemyHitBox = other.GetComponentInChildren<HitBox>();


            // Check if the EnemyInfo component is not null before calling TakeDamage
            if (enemyHitBox != null)
            {
                if(PlayerInfo.Instance != null)
                {
                    DamageEffect damageInstance = new DamageEffect((float)PlayerInfo.Instance.playerModifiers.damage, false);
                    EventManager.Instance.SubscribeToDamage(damageInstance, enemyHitBox.enemyInstance, other.ClosestPointOnBounds(transform.position));
                }
                else
                {
                    Debug.Log("PlayerInfo.Instance is null so no damage was dealt");
                }
                
                // Instantiate hitEffect GameObject
                //GameObject hitParticle = Instantiate(hitEffect, other.ClosestPointOnBounds(transform.position), Quaternion.identity);
            }
        }
    }
}
