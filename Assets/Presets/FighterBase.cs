using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterBase : ScriptableObject
{
    [SerializeField] protected string displayName;
    [SerializeField] protected int maxHealth;
    [SerializeField] protected float damage;
    [SerializeField] protected float speed;
    [SerializeField] protected List<FighterAbility> abilities;
    [SerializeField] protected List<Tag> tags;
    [SerializeField] protected Sprite sprite;

    public virtual float GetDamageMultiplier(Fighter f) {return 0f;}
    public string GetName() {return displayName;}
    public int GetMaxHealth() {return maxHealth;}
    public virtual float GetDamage() {return damage;}
    public List<FighterAbility> GetAbilities() {return abilities;}
    public List<Tag> GetTags() {return tags;}
    public bool HasTag(Tag tag) {return tags.Contains(tag);}
    public Sprite GetSprite() {return sprite;}
    public float GetSpeed() {return speed;}
    public string GetDescription() {return Ability.GetDescriptionFromList(abilities);}
}
