using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero Base", menuName = "Presets/Hero Base")]
public class FighterBase : ScriptableObject
{
    [SerializeField] protected string displayName;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected float damage;
    [SerializeField] protected float speed;
    [SerializeField] protected List<Tag> tags;
    [SerializeField] protected Sprite sprite;

    public virtual void SetType(Fighter fighter) {
        // fighter.displayName = displayName;
        // fighter.maxHealth = maxHealth;
        // fighter.health = maxHealth;
        // fighter.damage = damage;
    }

    public virtual void OnTakenDamage(Fighter source, float amount) {}

    public virtual void OnHeal() {}

    public virtual void OnAttack() {}

    public virtual void OnDeath(Fighter source) {}

    public void OnBattleFinished() 
    {
        Debug.LogWarning($"Not yet implemented FinishBattle");
    }

    public virtual Fighter DecideTarget(List<Hero> fighters)
    {
        return fighters[0];
    }

    public virtual Fighter DecideTarget(List<Monster> fighters)
    {
        return fighters[0];
    }

    public virtual float GetDamageMultiplier(Fighter f) {return 0f;}
    public string GetName() {return displayName;}
    public int GetMaxHealth() {return maxHealth;}
    public virtual float GetDamage() {return damage;}
    public List<Tag> GetTags() {return tags;}
    public Sprite GetSprite() {return sprite;}
    public float GetSpeed() {return speed;}
}
