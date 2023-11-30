using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterBase : ScriptableObject
{
    [SerializeField] private string displayName;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int cost;
    [SerializeField] private float baseDamage;
    [SerializeField] private int maxHealth;
    [SerializeField] private float speed = 1f;
    [SerializeField] private int tier = 1;
    [SerializeField] private List<FighterAbility> abilities;
    [SerializeField] private List<Tag> tags;

    public bool HasTag(Tag tag) {return tags.Contains(tag);}
    public string GetDescription() {return Ability.GetDescriptionFromList(abilities);}
    public int GetCost() {return cost;}
    public string GetName() {return displayName;}
    public int GetMaxHealth() {return maxHealth;}
    public virtual float GetDamage() {return baseDamage;}
    public List<FighterAbility> GetAbilities() {return abilities;}
    public List<Tag> GetTags() {return tags;}
    public Sprite GetSprite() {return sprite;}
    public float GetSpeed() {return speed;}
    public int GetTier() {return tier;}
}
