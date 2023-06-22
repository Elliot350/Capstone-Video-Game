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
    [SerializeField] protected float damageModifier = 0f;
    [SerializeField] protected List<FighterAbility> abilities;
    [SerializeField] protected List<Tag> tags;
    [SerializeField] protected Slider slider;
    [SerializeField] protected TextMeshProUGUI healthBarText;
    [SerializeField] protected Animator animator;
    [SerializeField] protected GameObject effectAnimator;
    private List<Effect> effects = new List<Effect>();
    protected bool isMonster;
    protected bool isBoss;
    [SerializeField] protected Image image, alertImage;

    protected float healthMultiplier = 1f;
    protected float damageMultiplier = 1f;
    protected Fighter lastAttacker;
    protected Room room;
    protected FighterBase fighterType;

    [SerializeField] protected ParticleSystem healParticles;

    // private List<Action> actions = new List<Action>();
    // private WaitForSeconds pause = new WaitForSeconds(1);

    private List<FighterAbility> abilitiesToRemove = new List<FighterAbility>();
    private List<FighterAbility> abilitiesToAdd = new List<FighterAbility>();

    public virtual void SetType(FighterBase fighterBase)
    {
        this.fighterType = fighterBase;
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
        SetTypes();
    }

    protected virtual void SetTypes()
    {
        Debug.LogWarning($"Did not set Types! ({this}) - ({gameObject})");
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
        health -= attack.GetDamage();
        SetHealthBar();
        HurtAnimation();
    }

    public virtual void Heal(float amount)
    {
        Debug.Log($"Healing ({health} + {amount})");
        foreach (FighterAbility a in abilities)
            a.OnHeal(this);
        health += amount;
        Debug.Log($"After {health}");
        // healParticles.Play();
        SetHealthBar();
    }

    private void SetHealthBar()
    {
        slider.value = health;
        healthBarText.text = $"{health}/{maxHealth}";
    }

    public virtual void Die(Damage attack)
    {
        foreach (FighterAbility a in abilities)
            a.OnDeath(attack);
        // if (this is Hero)
        // {
        //     foreach (Fighter f in FightManager.GetInstance().GetFighters())
        //         f.HeroDied(this);
        //     FightManager.GetInstance().GetRoom().HeroDied(this);
        // }
        // else if (this is Monster)
        // {
        //     foreach (Fighter f in FightManager.GetInstance().GetFighters())
        //         f.MonsterDied(this);
        //     FightManager.GetInstance().GetRoom().MonsterDied(this);
        // }
        // else 
        // {
        //     Debug.LogWarning($"Not sure what {this} is.");
        // }
        DeathAnimation();
    }

    public void HeroDied(Fighter f)
    {
        foreach (FighterAbility a in abilities)
        {
            a.OnHeroDied(this, f);
            a.OnFighterDied(this, f);
        }
    }

    public void MonsterDied(Fighter f)
    {
        foreach (FighterAbility a in abilities)
        {
            a.OnMonsterDied(this, f);
            a.OnFighterDied(this, f);
        }
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

    public List<Fighter> GetTargets(List<Fighter> fighters)
    {
        List<Fighter> targets = new List<Fighter>();

        foreach (FighterAbility a in abilities)
        {
            if (a.ModifiesTargets() && targets.Count <= a.DecideTargets(fighters).Count)
                targets = a.DecideTargets(fighters);
        }
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
    private Fighter source;
    private Fighter target;
    // TODO: Change this to a damage, damageMultiplier and damageModifier 
    private float damage;

    public Damage(Fighter source, Fighter target, float damage)
    {
        this.source = source;
        this.target = target;
        this.damage = damage;
    }

    public Fighter GetSource() {return source;}
    public Fighter GetTarget() {return target;}
    public void SetDamage(float amount) {damage = amount;}
    public float GetDamage() {return damage;}
}