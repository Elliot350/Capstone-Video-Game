using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Fighter : MonoBehaviour
{
    protected FighterBase fighterType;
    
    [Header("Stats")]
    [SerializeField] protected string displayName;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;
    [SerializeField] protected float healthMultiplier = 1f;
    [SerializeField] protected float damage;
    [SerializeField] protected float damageMultiplier = 1f;
    [SerializeField] protected float damageModifier = 0f;

    [Header("Abilities")]
    [SerializeField] protected List<FighterAbility> abilities;
    [SerializeField] protected List<Tag> tags;

    [Header("UI")]
    [SerializeField] protected Slider slider;
    [SerializeField] protected TextMeshProUGUI healthBarText;
    [SerializeField] protected Animator animator;
    [SerializeField] protected GameObject effectAnimator;
    [SerializeField] protected Image image;
    [SerializeField] protected Image alertImage;
    private List<Effect> effects = new List<Effect>();
    [SerializeField] protected ParticleSystem healParticles;

    [Header("Debug Stuff")]
    [SerializeField] private TextMeshProUGUI damageText;
    
    protected bool isMonster;
    protected bool isBoss;
    protected Room room;

    // Might change this in the future to Instantiate the fighterBase as well, not sure how well that will work
    public virtual void SetBase(FighterBase fighterBase)
    {
        this.fighterType = fighterBase;
        displayName = fighterBase.GetName();
        maxHealth = fighterBase.GetMaxHealth();
        health = maxHealth;
        damage = fighterBase.GetDamage();
        abilities = new List<FighterAbility>();
        foreach (FighterAbility a in fighterBase.GetAbilities())
            abilities.Add(Instantiate<FighterAbility>(a));
        tags = new List<Tag>(fighterBase.GetTags());
        slider.minValue = 0f;
        slider.maxValue = maxHealth;
        SetHealthBar();
        image.sprite = fighterBase.GetSprite();
        alertImage.gameObject.SetActive(false);
        SetTypes();
    }

    public void SetType(FighterBase fighterBase, Room room)
    {
        this.room = room;
        SetBase(fighterBase);
    }
    
    protected virtual void SetTypes()
    {
        Debug.LogWarning($"Did not set Types! ({this}) - ({gameObject})");
    }

    public virtual void TakeDamage(Damage attack)
    {
        ActivateAbilities((a) => a.OnTakenDamage(attack));
        if (attack.CalculatedDamage <= 0)
            return;
        health -= attack.CalculatedDamage;
        SetHealthBar();
        HurtAnimation();
    }

    public virtual void Heal(float amount)
    {
        ActivateAbilities((a) => a.OnHeal(this));
        health += amount;
        SetHealthBar();
        // healParticles.Play();
    }

    public virtual void IncreaseMaxHealth(float amount)
    {
        if (amount <= 0) return;
        maxHealth += amount;
        health += amount;
        ActivateAbilities((a) => a.OnHeal(this));
        SetHealthBar();
    }

    private void SetHealthBar()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        slider.maxValue = maxHealth;
        slider.value = health;
        healthBarText.text = $"{health}/{maxHealth}";
    }

    public virtual void Die(Damage attack)
    {
        FightManager manager = FightManager.GetInstance();
        // If this fighter is already dead, don't do anythin
        if (manager.GetDead().Contains(this)) return;

        ActivateAbilities((a) => a.OnDeath(attack));
        DeathAnimation();
        
        if (!manager.GetFighters().Remove(this))
            Debug.LogWarning($"Can't remove {this}");
        Invoke("MoveToGraveyard", manager.FastForwarding() ? 0f : 0.3f);
        
        if (isMonster)
        {
            if (!manager.GetMonsters().Remove(this))
                Debug.LogWarning($"Can't remove monster ({this})");
        }
        else
        {
            if (!manager.GetHeroes().Remove(this))
                Debug.LogWarning($"Can't remove hero ({this})");
            PartyManager.GetInstance().HeroDied(this.GetComponent<Hero>());
        }

        manager.GetDead().Add(this);
    }

    private void MoveToGraveyard()
    {
        transform.SetParent(FightManager.GetInstance().GetDeadHolder().transform);
    }

    public void FighterDied(Fighter f) {ActivateAbilities((a) => a.OnFighterDied(this, f));}
    public void FinishBattle()  {ActivateAbilities((a) => a.BattleEnd(this));}
    public void StartBattle() {ActivateAbilities((a) => a.BattleStart(this));}
    public void StartTurn() {ActivateAbilities((a) => a.TurnStart(this));}
    public void EndTurn() {ActivateAbilities((a) => a.TurnEnd(this));}

    public void ActivateAbilities(Action<FighterAbility> action)
    {
        foreach (FighterAbility a in abilities)
            action(a);
    }

    public float CalculateDamage()
    {
        damageText.text = $"{damage} x {CalculateDamageMultiplier()} + {damageModifier}";
        return (damage * CalculateDamageMultiplier()) + damageModifier;
    }

    protected virtual float CalculateDamageMultiplier()
    {
        float multiplier = 1f + room.GetDamageMultiplier(this);
        foreach (FighterAbility a in abilities)
            multiplier += a.GetDamageMultiplier(this);
        return Mathf.Max(multiplier, 0);
    }

    // TODO: Make a better way to do this
    public List<Fighter> GetTargets(List<Fighter> fighters)
    {
        List<Fighter> targets = new List<Fighter>();

        ActivateAbilities((a) => {
            if (a.ModifiesTargets() && targets.Count <= a.DecideTargets(fighters).Count)
                targets = a.DecideTargets(fighters);
        });

        if (targets.Count == 0)
            targets.Add(fighters[0]);
        return targets;
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
        if (FightManager.GetInstance().FastForwarding() || !animator.isActiveAndEnabled)
            return;
        animator.SetBool("Monster", isMonster);
        animator.SetTrigger("Attack");
    }

    public virtual void HurtAnimation()
    {
        if (FightManager.GetInstance().FastForwarding() || !animator.isActiveAndEnabled)
            return;
        animator.SetBool("Monster", isMonster);
        animator.SetTrigger("Hurt");
    }

    public virtual void DeathAnimation()
    {
        if (FightManager.GetInstance().FastForwarding() || !animator.isActiveAndEnabled)
            return;
        animator.SetTrigger("Dead");
    }

    public virtual void ReviveAnimation()
    {
        // TODO: Might have to change this because otherwise it would be stuck on the Dead animaition
        if (FightManager.GetInstance().FastForwarding() || !animator.isActiveAndEnabled)
            return;
        animator.SetTrigger("Revive");
    }

    public virtual Effect PlayEffect(string animationName)
    {
        if (FightManager.GetInstance().FastForwarding())
            return null;
        Effect newEffect = Instantiate(effectAnimator, transform).GetComponent<Effect>();
        effects.Add(newEffect);
        newEffect.PlayEffect(animationName);
        return newEffect;
    }

    public virtual Effect PlayEffect(string animationName, Effect effect)
    {
        if (!effects.Contains(effect))
            return effect;
        effect.PlayEffect(animationName);
        return effect;
    }

    public void EffectDone(Effect effect)
    {
        if (!effects.Contains(effect))
            return;
        effects.Remove(effect);
        Destroy(effect.gameObject);
    }

    public FighterBase GetFighterType() {return fighterType;}
    public bool IsMonster() {return isMonster;}
    public virtual Sprite GetSprite() {return image.sprite;}
    public Room GetRoom() {return room;}
    public string GetName() {return displayName;}
    public float GetHealth() {return health;}
    public float GetMaxHealth() {return maxHealth;}
    public float GetDamage() {return damage;}
    public List<Tag> GetTags() {return tags;}
    public virtual float GetSpeed() {return fighterType.GetSpeed();}
    public string GetDescription() {return Ability.GetDescriptionFromList(abilities);}
    public List<FighterAbility> GetAbilities() {return abilities;}
}

public class Damage
{
    public Fighter Source {get; private set;}
    public Fighter Target {get; private set;}
    public float BaseDamage {get; set;}
    public float DamageMultiplier {get; set;}
    public float DamageModifier {get; set;}
    public float CalculatedDamage 
    {
        get {return (float) Math.Round(BaseDamage * DamageMultiplier + DamageModifier, 2);}
    }

    public Damage(Fighter source, Fighter target, float damage, float damageMultiplier, float damageModifier)
    {
        Source = source;
        Target = target;
        BaseDamage = damage;
        DamageMultiplier = damageMultiplier;
        DamageModifier = damageModifier;
    }
    
    public Damage(Fighter target, float damage) : this(null, target, damage, 1f, 0f) {}
    public Damage(Fighter source, Fighter target, float damage) : this(source, target, damage, 1f, 0f) {}
    public Damage(Fighter newTarget, Damage damage) : this(damage.Source, newTarget, damage.BaseDamage, damage.DamageMultiplier, damage.DamageModifier) {}

}