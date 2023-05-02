using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : MonoBehaviour
{
    [SerializeField] public string displayName;
    [SerializeField] public int maxHealth;
    [SerializeField] public int health;
    [SerializeField] public int damage;
    [SerializeField] public int gold;
    [SerializeField] public SpriteRenderer spriteRenderer;

    public virtual void TakeDamage(int amount) 
    {
        Debug.LogWarning($"Not yet implemented TakeDamage");
    }

    public virtual void Heal(int amount) 
    {
        Debug.LogWarning($"Not yet implemented Heal");
    }

    public virtual void Attack() 
    {
        Debug.LogWarning($"Not yet implemented Attack");
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
}
