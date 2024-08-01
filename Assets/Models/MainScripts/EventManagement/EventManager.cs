using UnityEngine;
using UnityEngine.Events;
using System;


public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    // Events:
    public event Action<DamageEffect, Enemy, Vector3, AudioClip> playerDamageEnemyEvent;
    public event Action<Enemy> enemyOnDieEvent;

    // In the class that triggers the event
    public delegate void WeaponInstanceHandler(Weapon weapon);
    public static event WeaponInstanceHandler OnWeaponInstanceSet;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Lock the frame rate to 60 frames per second
        Application.targetFrameRate = 120;

        // Example: Subscribe a method to the event
        playerDamageEnemyEvent += PlayerDamageEnemyEvent;
        enemyOnDieEvent += EnemyOnDieEvent;
    }

    private void PlayerDamageEnemyEvent(DamageEffect damageInstance, Enemy enemyInstance, Vector3 hitPoint, AudioClip hitSound = null)
    {
        Debug.Log("PlayerDamageEnemyEvent");
        // Handle the event, e.g., apply damage to the enemy
        enemyInstance.OnHitBoxTriggerEnter(damageInstance);
        PopUpGenerator.Instance.CreatePopUp(hitPoint, $"{damageInstance.damage}", Color.yellow, hitSound);
    }

    private void EnemyOnDieEvent(Enemy enemyInstance)
    {
        // Handle the event, e.g., apply damage to the enemy
        // Note: Since there might be multiple enemies, you need to handle this event appropriately for each enemy.
        // If you want to notify all enemies, you might need to store a list of enemies and iterate through them.
        enemyInstance.OnDie();
    }

    // Example method for subscribing to the OnDie event of a specific enemy
    public void SubscribleToEnemyOnDie(Enemy enemyInstance)
    {
        // You may want to pass the specific enemy instance to the subscriber method
        enemyOnDieEvent?.Invoke(enemyInstance);
    }

    public void SubscribeToDamage(DamageEffect damageInstance, Enemy enemyInstance, Vector3 hitPoint, AudioClip hitSound = null)
    {
        // Example: Trigger the event with the provided parameters
        playerDamageEnemyEvent?.Invoke(damageInstance, enemyInstance, hitPoint, hitSound);
    }




    public void SetWeaponInstance(Weapon weapon)
    {
        // Set the weapon instance and trigger the event
        OnWeaponInstanceSet?.Invoke(weapon);
    }
}
