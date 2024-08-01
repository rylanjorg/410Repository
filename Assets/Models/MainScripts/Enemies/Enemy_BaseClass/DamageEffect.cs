using UnityEngine;

[System.Serializable]
public class DamageEffect 
{
    [Header("Inscribed")]
    public float damage = 1.0f;
    public bool knockback = true;

    public DamageEffect(float damage, bool knockback)
    {
        this.damage = damage;
        this.knockback = knockback;
    }
}
