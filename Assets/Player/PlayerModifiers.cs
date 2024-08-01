using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerModifiers 
{
    public float basePlayerSpeed;
    public float playerSpeed;
    public float lootingMultiplier = 1f;
    public float defense = 0f;
    public float damage = 5f;
    public float attackSpeed = 1f;
    public float health;

    public float maxHealth = 100f;
    public float chipSpeed = 2f; 
    public int currency = 0;
  
}
