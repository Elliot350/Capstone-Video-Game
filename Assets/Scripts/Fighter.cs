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
    // Current max health (including permanant changes)
    [SerializeField] protected float maxHealth;
    // Temporary modifier to max health (this can change from the fight changing)
    [SerializeField] protected float maxHealthModifier;
    private float previousMaxHealth;
    // Current health the fighter has
    [SerializeField] protected float health;
    // Current damage (including permanant changes)
    [SerializeField] protected float damage;
    // Temporary modifier to damage (this can change from the fight changing)
    [SerializeField] protected float damageModifier;
    private float previousDamage;
    [SerializeField] protected int tier;

    [Space(10)]
    [Header("Abilities")]
    [SerializeField] protected List<FighterAbility> abilities;
    [SerializeField] protected List<Tag> tags;

    [Space(10)]
    [Header("UI")]
    [SerializeField] protected Slider slider;
    [SerializeField] protected TextMeshProUGUI healthBarText;
    [SerializeField] protected Animator animator;
    [SerializeField] protected GameObject effectAnimator;
    [SerializeField] protected Image image;
    [SerializeField] protected Image alertImage;
    private List<Effect> effects = new List<Effect>();
    [SerializeField] protected ParticleSystem healParticles;

    [Space(10)]
    [Header("Debug Stuff")]
    [SerializeField] private TextMeshProUGUI damageText;
    
    public bool IsMonster;
    public bool IsBoss;
    public bool IsDead;
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
        tier = fighterBase.GetTier();
        alertImage.gameObject.SetActive(false);
        SetTypes();
    }

    public void SetBase(FighterBase fighterBase, Room room)
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
        Debug.Log($"Taking damage for {attack.CalculatedDamage}");
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
        maxHealthModifier += amount;
        health += amount;
        ActivateAbilities((a) => a.OnHeal(this));
        SetHealthBar();
    }

    private void SetHealthBar()
    {
        float tmp = GetMaxHealth();
        health = Mathf.Clamp(health, 0, tmp);
        slider.maxValue = tmp;
        slider.value = health;
        healthBarText.text = $"{health}/{tmp}";
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
        
        if (IsMonster)
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
        IsDead = true;
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
    public void MonsterSummoned(Fighter summoned) {ActivateAbilities((a) => a.MonsterSummoned(this, summoned));}

    private void ActivateAbilities(Action<FighterAbility> action)
    {
        foreach (FighterAbility a in abilities)
            action(a);
    }

    public void ResetStats()
    {
        previousDamage = damage + damageModifier;
        previousMaxHealth = maxHealth + maxHealthModifier;
        damageModifier = 0f;
        maxHealthModifier = 0f;
    }

    // This method assumes that this fighter and all other fighters have has their modifiers set to 0
    public void CalculateStats()
    {
        CalculateDamage();
        CalculateMaxHealth();
        if (GetMaxHealth() > previousMaxHealth)
        {
            health += GetMaxHealth() - previousMaxHealth;
        }
        SetHealthBar();
    }

    private void CalculateDamage()
    {
        if (room != null)
            room.CalculateDamage(this);
        ActivateAbilities((a) => a.CalculateDamage(this));
        damageText.text = $"{damage} + {damageModifier}\n{maxHealth} + {maxHealthModifier}";
    }

    public void IncreaseDamageModifier(float amount)
    {
        damageModifier += amount;
    }

    private void CalculateMaxHealth()
    {
        if (room != null)
            room.CalculateMaxHealth(this);
        ActivateAbilities((a) => a.CalculateMaxHealth(this));
    }

    public void IncreaseMaxHealthModifier(float amount)
    {
        maxHealthModifier += amount;
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
        animator.SetBool("Monster", IsMonster);
        animator.SetTrigger("Attack");
    }

    public virtual void HurtAnimation()
    {
        if (FightManager.GetInstance().FastForwarding() || !animator.isActiveAndEnabled)
            return;
        animator.SetBool("Monster", IsMonster);
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
    public string GetName() {return displayName;}
    public string GetDescription() {return Ability.GetDescriptionFromList(abilities);}
    public float GetBaseMaxHealth() {return fighterType.GetMaxHealth();}
    public float GetMaxHealth() {return maxHealth + maxHealthModifier;}
    public float GetHealth() {return health;}
    public float GetDamage() {return damage + damageModifier;}
    public float GetBaseDamage() {return fighterType.GetDamage();}
    public List<FighterAbility> GetAbilities() {return abilities;}
    public List<Tag> GetTags() {return tags;}
    public virtual Sprite GetSprite() {return image.sprite;}
    public virtual float GetSpeed() {return fighterType.GetSpeed();}
    public Room GetRoom() {return room;}
}

public class Damage
{
    public Fighter Source {get; private set;}
    public Fighter Target {get; private set;}
    public float BaseDamage {get; set;}
    public float DamageTempModifier {get; set;}
    public float DamageModifier {get; set;}
    public float CalculatedDamage 
    {
        get {return (float) Math.Round(BaseDamage + DamageTempModifier + DamageModifier, 2);}
    }

    public Damage(Fighter source, Fighter target, float damage, float damageMultiplier, float damageModifier)
    {
        Source = source;
        Target = target;
        BaseDamage = damage;
        DamageTempModifier = damageMultiplier;
        DamageModifier = damageModifier;
    }
    
    public Damage(Fighter target, float damage) : this(null, target, damage, 0f, 0f) {}
    public Damage(Fighter source, Fighter target, float damage) : this(source, target, damage, 0f, 0f) {}
    public Damage(Fighter newTarget, Damage damage) : this(damage.Source, newTarget, damage.BaseDamage, damage.DamageTempModifier, damage.DamageModifier) {}

}