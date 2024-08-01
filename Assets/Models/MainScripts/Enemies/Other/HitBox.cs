using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;

public class HitBox : MonoBehaviour
{
    [SerializeReference]
    public Enemy enemyInstance;
    /*public event Action<DamageEffect> onTriggerEnterEvent;
    private Collider hitBoxCollider;

    private void Start()
    {
        onTriggerEnterEvent += CallEnemyHitInstance;
    }

    private void CallEnemyHitInstance(DamageEffect damageInstance)
    {
        enemyInstance.OnHitBoxTriggerEnter(hitBoxCollider, damageInstance);
    }

    public void SubscribeToDamage(DamageEffect damageInstance)
    {
        Debug.Log($"HitBox Enter Event -> SubscribeToDamage. Enemy Name: {enemyInstance.name}, Collider Name: {other.name}, Tag: {other.tag}, Layer: {other.gameObject.layer}.");

        if (enemyInstance != null && onTriggerEnterEvent != null)
        {
            hitBoxCollider = other;
            onTriggerEnterEvent(damageInstance);
        }
    }*/
}
