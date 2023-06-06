using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Fighter : MonoBehaviour
{
    [SerializeField] protected string displayName;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;
    [SerializeField] protected float damage;
    [SerializeField] protected List<FighterAbility> abilities;
    [SerializeField] protected List<Tag> tags;
    [SerializeField] protected Slider slider;
    [SerializeField] protected Animator animator;
    protected bool isMonster;
    [SerializeField] protected Image image, alertImage;

    protected float healthMultiplier = 1f;
    protected float damageMultiplier = 1f;
    [SerializeField] protected float damageModifier = 0f;
    protected Fighter lastAttacker;
    protected Room room;
    protected FighterBase fighterBase;

    // private List<Action> actions = new List<Action>();
    // private WaitForSeconds pause = new WaitForSeconds(1);

    private List<FighterAbility> abilitiesToRemove = new List<FighterAbility>();
    private List<FighterAbility> abilitiesToAdd = new List<FighterAbility>();

    public void SetType(FighterBase fighterBase)
    {
        this.fighterBase = fighterBase;
        displayName = fighterBase.GetName();
        maxHealth = fighterBase.GetMaxHealth();
        health = maxHealth;
        damage = fighterBase.GetDamage();
        abilities = new List<FighterAbility>(fighterBase.GetAbilities());
        tags = new List<Tag>(fighterBase.GetTags());
        slider.minValue = 0f;
        slider.maxValue = maxHealth;
        SetHealthBar();
        image.sprite = fighterBase.GetSprite();
        alertImage.gameObject.SetActive(false);
        SetAnimator();
    }

    protected virtual void SetAnimator()
    {
        Debug.LogWarning($"Did not set Animator! ({this}) - ({gameObject})");
    }

    public void SetType(FighterBase fighterBase, Room room)
    {
        this.room = room;
        SetType(fighterBase);
    }

    public virtual void TakeDamage(Damage attack)
    {
        foreach (FighterAbility a in abilities)
            a.OnTakenDamage(attack);
        health -= attack.damage;
        SetHealthBar();
        HurtAnimation();
    }

    public virtual void Heal(float amount)
    {
        foreach (FighterAbility a in abilities)
            a.OnHeal(this);
        health += amount;
        SetHealthBar();
    }

    private void SetHealthBar()
    {
        slider.value = health;
    }

    public void Die(Damage attack)
    {
        foreach (FighterAbility a in abilities)
            a.OnDeath(attack);
        if (this is Hero)
        {
            foreach (Fighter f in FightManager.GetInstance().GetFighters())
                f.HeroDied(this);
            FightManager.GetInstance().GetRoom().HeroDied(this);
        }
        else if (this is Monster)
        {
            foreach (Fighter f in FightManager.GetInstance().GetFighters())
                f.MonsterDied(this);
            FightManager.GetInstance().GetRoom().MonsterDied(this);
        }
        DeathAnimation();
    }

    public void HeroDied(Fighter f)
    {
        foreach (FighterAbility a in abilities)
            a.OnHeroDied(this, f);
    }

    public void MonsterDied(Fighter f)
    {
        foreach (FighterAbility a in abilities)
            a.OnMonsterDied(this, f);
    }

    public void FinishBattle() 
    {
        foreach (FighterAbility a in abilities)
            a.OnBattleFinished(this);
    }

    public void StartBattle()
    {
        foreach (FighterAbility a in abilities)
            a.OnBattleStarted(this);
    }

    public float CalculateDamage()
    {
        return (damage * CalculateDamageMultiplier()) + damageModifier;
    }

    protected virtual float CalculateDamageMultiplier()
    {
        float multiplier = 1f + room.GetDamageMultiplier(this);
        foreach (FighterAbility a in abilities)
            multiplier += a.GetDamageMultiplier(this);
        return Mathf.Max(multiplier, 0);
    }

    public void AddDamage(float amount)
    {
        damageModifier += amount;
    }

    public bool HasTag(Tag t)
    {
        return tags.Contains(t);
    }

    public void AddTag(Tag t)
    {
        if (tags.Contains(t)) 
            return;
        tags.Add(t);
    }

    public virtual void AttackAnimation()
    {
        if (FightManager.GetInstance().FastForwarding())
            return;
        animator.SetBool("Monster", isMonster);
        animator.SetTrigger("Attack");
    }

    public virtual void HurtAnimation()
    {
        if (FightManager.GetInstance().FastForwarding())
            return;
        animator.SetBool("Monster", isMonster);
        animator.SetTrigger("Hurt");
    }

    public virtual void DeathAnimation()
    {
        if (FightManager.GetInstance().FastForwarding())
            return;
        animator.SetTrigger("Dead");
    }

    public virtual Sprite GetSprite() {return image.sprite;}
    public Room GetRoom() {return room;}
    public string GetName() {return displayName;}
    public float GetHealth() {return health;}
    public float GetMaxHealth() {return maxHealth;}
    public float GetDamage() {return damage;}
    public List<Tag> GetTags() {return tags;}
    public virtual float GetSpeed() {return fighterBase.GetSpeed();}
    public string GetDescription()
    {
        if (abilities.Count == 0)
            return "No abilities";
        string text = "";
        foreach (FighterAbility a in abilities)
            text += a.GetAbility();
        return text;
    }
    public List<FighterAbility> GetAbilities() {return abilities;}
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