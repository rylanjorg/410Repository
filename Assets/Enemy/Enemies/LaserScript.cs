using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public DamageEffect damageEffect;
    public LayerMask damageLayer; // Layer to damage
    public LayerMask collideLayer; // Layer to collide with
    public float destroyTime = 0.2f;
    public float damagePopUpYoffset = 1.0f;
    public AudioClip hitSound;

    private float spawnTime;

    void Start()
    {
        // Record the time when the object is created
        spawnTime = Time.time;
    }

    void Update()
    {
        // Check if the object has existed for more than 7 seconds
        if (Time.time - spawnTime > destroyTime)
        {
            Destroy(gameObject);
        }
    }

    // OnTriggerEnter method that takes in a collider 
    private void OnTriggerEnter(Collider other)
    {
        // if it collides with the damage layer
        if (((1 << other.gameObject.layer) & damageLayer) != 0)
        {
            

            if(other.gameObject.layer == 7)
            {
                // Damage player
                if(PlayerInfo.Instance != null)
                PlayerInfo.Instance.TakeDamage(damageEffect.damage);
            }
            
            if(other.gameObject.layer == 10)
            {
                HitBox hitBox = other.GetComponent<HitBox>();
                if(hitBox != null)
                {
                    Debug.Log("Going to subscribe to damage enemy");
                    Enemy enemy = hitBox.enemyInstance;
                    EventManager.Instance.SubscribeToDamage(damageEffect, enemy, other.ClosestPointOnBounds(transform.position ) + new Vector3(0, damagePopUpYoffset, 0), hitSound);
                }
            }
            
           
            // Destroy the game object
            Destroy(gameObject);
        }
        // if it collides with the collide layer, makes sure the lasers have enough time to get away from the turret before destroying them
        else if (((1 << other.gameObject.layer) & collideLayer) != 0)
        {
          
            //EventManager.Instance.SubscribeToDamage(damageEffect, );
            // Destroy the game object
            Destroy(gameObject);
        }
    }
}