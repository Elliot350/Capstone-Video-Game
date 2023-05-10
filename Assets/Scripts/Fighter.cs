using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] protected string displayName;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;
    [SerializeField] protected float damage;
    public float healthMultiplier = 1f, damageMultiplier = 1f;

    public virtual void TakeDamage(float amount) 
    {
        health -= amount;
        if (health <= 0)
            Die();
    }

    public virtual void Heal(float amount) 
    {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
    }

    public virtual void Attack(List<Monster> fighters) 
    {
        fighters[0].TakeDamage(Mathf.RoundToInt(damage * damageMultiplier));
    }

    public virtual void Attack(List<Hero> fighters) 
    {
        Debug.Log($"{this} is attacking for {damage * damageMultiplier}");
        fighters[0].TakeDamage(Mathf.RoundToInt(damage * damageMultiplier));
    }

    public virtual void Die() 
    {
        Debug.LogWarning($"Not yet implemented Die");
    }

    public void FinishBattle() 
    {
        Debug.LogWarning($"Not yet implemented FinishBattle");
    }

    public string GetName()
    {
        return displayName;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetDamage()
    {
        return damage;
    }

    public virtual float GetSpeed()
    {
        return 1f;
    }
}
