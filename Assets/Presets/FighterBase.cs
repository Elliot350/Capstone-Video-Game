using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterBase : ScriptableObject
{
    [SerializeField] protected string displayName;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected float damage;
    [SerializeField] protected float speed;
    [SerializeField] protected List<Ability> abilities;
    [SerializeField] protected List<Tag> tags;
    [SerializeField] protected Sprite sprite;

    public virtual void SetType(Fighter fighter) {
        // fighter.displayName = displayName;
        // fighter.maxHealth = maxHealth;
        // fighter.health = maxHealth;
        // fighter.damage = damage;
    }

    public virtual void OnTakenDamage(Damage attack) {}

    public virtual void OnHeal() {}

    public virtual void OnAttack() {}

    public virtual void OnDeath(Damage attack) {}

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
    public List<Ability> GetAbilities() {return abilities;}
    public List<Tag> GetTags() {return tags;}
    public Sprite GetSprite() {return sprite;}
    public float GetSpeed() {return speed;}
    public string GetDescription()
    {
        if (abilities.Count == 0)
            return "No abilities";
        string text = "";
        foreach (Ability a in abilities)
            text += a.Format();
        return text;
    }
}
