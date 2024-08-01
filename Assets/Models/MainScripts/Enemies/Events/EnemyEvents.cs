using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyEvents : MonoBehaviour
{
    public static EnemyEvents current;

    private void Awake()
    {
        current = this;
    }
    
    public event Action onEnemyDie;
    public void OnEnemyDie()
    {
        if(onEnemyDie != null)
        {
            onEnemyDie();
        }
    }

    public void OnHitBoxTriggerEnter(Collider other)
    {
        /*if (other.gameObject.CompareTag("Player"))
        {
            OnEnemyDie();
        }*/
    }
}
