using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] protected string displayName;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int health;
    [SerializeField] protected int damage;
    [SerializeField] protected int gold;
    [SerializeField] protected SpriteRenderer spriteRenderer;

    public void TakeDamage(int amount) 
    {
        Debug.LogWarning($"Not yet implemented TakeDamage");
    }

    public void Heal(int amount) 
    {
        Debug.LogWarning($"Not yet implemented Heal");
    }

    public void Attack() 
    {
        Debug.LogWarning($"Not yet implemented Attack");
    }

    public void Die() 
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
}
