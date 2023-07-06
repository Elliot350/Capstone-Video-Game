using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterBase : ScriptableObject
{
    [SerializeField] protected string displayName;
    [SerializeField] protected Sprite sprite;
    [SerializeField] protected int cost;
    [SerializeField] protected float baseDamage;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected float speed;
    [SerializeField] protected List<FighterAbility> abilities;
    [SerializeField] protected List<Tag> tags;

    public int GetCost() {return cost;}
    public string GetName() {return displayName;}
    public int GetMaxHealth() {return maxHealth;}
    public virtual float GetDamage() {return baseDamage;}
    public List<FighterAbility> GetAbilities() {return abilities;}
    public List<Tag> GetTags() {return tags;}
    public bool HasTag(Tag tag) {return tags.Contains(tag);}
    public Sprite GetSprite() {return sprite;}
    public float GetSpeed() {return speed;}
    public string GetDescription() {return Ability.GetDescriptionFromList(abilities);}
}
