using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero Base", menuName = "Presets/Hero Base")]
public class FighterBase : ScriptableObject
{
    [SerializeField] protected string displayName;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int damage;
    [SerializeField] protected Sprite sprite;

    public virtual void SetType(Fighter fighter) {
        fighter.displayName = displayName;
        fighter.maxHealth = maxHealth;
        fighter.health = maxHealth;
        fighter.damage = damage;
    }

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

    public virtual void Die(Fighter fighter) 
    {
        Debug.LogWarning($"Not yet implemented Die");
    }

    public void FinishBattle() 
    {
        Debug.LogWarning($"Not yet implemented FinishBattle");
    }

    public string GetName() {return displayName;}
    public int GetMaxHealth() {return maxHealth;}
    public int GetDamage() {return damage;}
    public Sprite GetSprite() {return sprite;}
}
