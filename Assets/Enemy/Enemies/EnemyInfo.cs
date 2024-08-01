using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyInfo : MonoBehaviour
{
    [HideInInspector]
    public float health;
    [HideInInspector]
    public float baseHealth;
    public float maxHealth;
    public int currencyDropped;

    void Start()
    {
        health = maxHealth;
        baseHealth = maxHealth;
    }

    void Update()
    {
        if (health <= 0f)
        {
            PlayerInfo.Instance.playerModifiers.currency += Mathf.FloorToInt(currencyDropped * PlayerInfo.Instance.playerModifiers.lootingMultiplier);
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
}
