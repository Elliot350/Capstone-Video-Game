using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] public string displayName;
    [SerializeField] public float maxHealth;
    [SerializeField] public float health;
    [SerializeField] public float damage;
    [SerializeField] public int gold;
    [SerializeField] public SpriteRenderer spriteRenderer;
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

    public void ShowFighter(Room room)
    {
        Debug.Log($"Showing {this}");
        transform.position = room.transform.position;
        spriteRenderer.gameObject.SetActive(true);
    }

    public void ShowFighter()
    {
        spriteRenderer.gameObject.SetActive(true);
    }

    public string GetName()
    {
        return displayName;
    }

    public virtual float GetSpeed()
    {
        return 1f;
    }
}
