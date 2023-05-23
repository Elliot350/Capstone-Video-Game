using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour
{
    [SerializeField] protected string displayName;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;
    [SerializeField] protected float damage;
    [SerializeField] protected List<Ability> abilities;
    [SerializeField] protected List<Tag> tags;
    [SerializeField] protected Slider slider;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Image image, alertImage;

    protected float healthMultiplier = 1f;
    protected float damageMultiplier = 1f;
    protected Fighter lastAttacker;
    protected Room room;
    protected FighterBase fighterBase;

    public void SetType(FighterBase fighterBase)
    {
        this.fighterBase = fighterBase;
        displayName = fighterBase.GetName();
        maxHealth = fighterBase.GetMaxHealth();
        health = maxHealth;
        damage = fighterBase.GetDamage();
        abilities = new List<Ability>(fighterBase.GetAbilities());
        tags = new List<Tag>(fighterBase.GetTags());
        slider.minValue = 0f;
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        image.sprite = fighterBase.GetSprite();
        alertImage.gameObject.SetActive(false);
    }

    public void SetType(FighterBase fighterBase, Room room)
    {
        this.room = room;
        SetType(fighterBase);
    }

    public virtual void TakeDamage(Damage attack)
    {
        health -= attack.damage;
        slider.value = health;
        foreach (Ability a in abilities)
            a.OnTakenDamage(attack);
        if (health <= 0)
            Die(attack);
    }

    public virtual void Heal(float amount) 
    {
        health += amount;
        foreach (Ability a in abilities)
            a.OnHeal(this);
        if (health > maxHealth)
            health = maxHealth;
    }

    public virtual void Attack(List<Monster> fighters) 
    {
        animator.SetTrigger("Attack");
        foreach (Ability a in abilities)
            a.OnAttack(this);
    }

    public virtual void Attack(List<Hero> fighters) 
    {
        animator.SetTrigger("Attack");
        foreach (Ability a in abilities)
            a.OnAttack(this);
    }

    public virtual void DoneAttack()
    {
        alertImage.gameObject.SetActive(false);
    }

    public virtual void Die(Damage attack) 
    {
        FightManager.GetInstance().FighterDied(this);
        foreach (Ability a in abilities)
            a.OnDeath(attack);
        animator.SetTrigger("Dead");
    }

    public void FinishBattle() 
    {
        foreach (Ability a in abilities)
            a.OnBattleFinished(this);
    }

    public void StartBattle()
    {
        foreach (Ability a in abilities)
            a.OnBattleStarted(this);
    }

    protected virtual float CalculateDamageMultiplier()
    {
        float multiplier = 1f + room.roomBase.CalculateDamageMultiplier(this);
        foreach (Ability a in abilities)
            multiplier += a.GetDamageMultiplier(this);
        Debug.Log($"Multiplier is {multiplier}");
        return multiplier;
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }

    public virtual Sprite GetSprite() {return null;}
    public Room GetRoom() {return room;}
    public string GetName() {return displayName;}
    public float GetHealth() {return health;}
    public float GetMaxHealth() {return maxHealth;}
    public float GetDamage() {return damage;}
    public List<Tag> GetTags() {return tags;}
    public virtual float GetSpeed() {return 1f;}
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

public class Damage
{
    public Fighter source;
    public Fighter target;
    public float damage;

    public Damage(Fighter source, Fighter target, float damage)
    {
        this.source = source;
        this.target = target;
        this.damage = damage;
    }
}