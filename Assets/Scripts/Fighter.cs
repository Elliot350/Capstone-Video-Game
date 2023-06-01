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
    [SerializeField] protected List<FighterAbility> abilities;
    [SerializeField] protected List<Tag> tags;
    [SerializeField] protected Slider slider;
    [SerializeField] protected Animator animator;
    protected bool isMonster;
    [SerializeField] protected Image image, alertImage;

    protected float healthMultiplier = 1f;
    protected float damageMultiplier = 1f;
    protected Fighter lastAttacker;
    protected Room room;
    protected FighterBase fighterBase;

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
        HurtAnimation();
        health -= attack.damage;
        SetHealthBar();
        if (health <= 0)
            Die(attack);
    }

    public virtual void Heal(float amount) 
    {
        health += amount;
        foreach (FighterAbility a in abilities)
            a.OnHeal(this);
        if (health > maxHealth)
            health = maxHealth;
        SetHealthBar();
    }

    public IEnumerator StartAttack(List<Fighter> fighters)
    {
        CatchUpAbilities();
        List<Fighter> targets = DecideTargets(fighters);
        foreach (Fighter target in targets)
        {
            Attack(target);
            yield return new WaitForSeconds(2);
        }
    }

    public virtual void Attack(List<Fighter> fighters) 
    {
        CatchUpAbilities();
        List<Fighter> targets = DecideTargets(fighters);
        foreach (Fighter target in targets)
        {
            float damageMultiplier = CalculateDamageMultiplier();
            float attackDamage = damage * damageMultiplier;
            Damage attack = new Damage(this, target, attackDamage);
            foreach (FighterAbility a in abilities)
                a.OnAttack(attack);
            CatchUpAbilities();
            AttackAnimation();
            target.TakeDamage(attack);
        }
        
    }

    public virtual void Attack(Fighter f)
    {
        float damageMultiplier = CalculateDamageMultiplier();
        float attackDamage = damage * damageMultiplier;
        Damage attack = new Damage(this, f, attackDamage);
        foreach (FighterAbility a in abilities)
            a.OnAttack(attack);
        CatchUpAbilities();
        AttackAnimation();
        f.TakeDamage(attack);
    }

    protected virtual List<Fighter> DecideTargets(List<Fighter> fighters)
    {
        List<Fighter> targets = new List<Fighter>();
        targets.Add(fighters[0]);

        foreach (FighterAbility a in abilities)
        {
            if (a.DecideTargets(fighters).Count >= targets.Count)
                targets = a.DecideTargets(fighters);
        }
        return targets;
    }

    public virtual void DoneAttack()
    {
        alertImage.gameObject.SetActive(false);
    }

    public virtual void Die(Damage attack) 
    {
        FightManager.GetInstance().FighterDied(this);
        foreach (FighterAbility a in abilities)
            a.OnDeath(attack);
        DeathAnimation();
    }

    private void SetHealthBar()
    {
        slider.value = health;
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

    protected virtual float CalculateDamageMultiplier()
    {
        float multiplier = 1f + room.GetDamageMultiplier(this);
        foreach (FighterAbility a in abilities)
            multiplier += a.GetDamageMultiplier(this);
        return Mathf.Max(multiplier, 0);
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
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

    protected void CatchUpAbilities()
    {
        // Debug.Log($"Catching up abilities ({abilities.Count} + {abilitiesToAdd.Count} - {abilitiesToRemove.Count})");
        foreach (FighterAbility a in abilitiesToAdd)
            abilities.Add(a);
        foreach (FighterAbility a in abilitiesToRemove)
        {
            if (abilities.Contains(a))
                abilities.Remove(a);
        }
        abilitiesToAdd.Clear();
        abilitiesToRemove.Clear();
        // Debug.Log(abilities.Count);
    }

    public void AddAbility(FighterAbility a)
    {
        abilitiesToAdd.Add(a);
    }

    public void RemoveAbility(FighterAbility a)
    {
        // Debug.Log($"Adding {a} ({a.GetName()}) to remove");
        abilitiesToRemove.Add(a);
    }

    protected virtual void AttackAnimation()
    {
        animator.SetBool("Monster", isMonster);
        animator.SetTrigger("Attack");
    }

    protected virtual void HurtAnimation()
    {
        animator.SetBool("Monster", isMonster);
        animator.SetTrigger("Hurt");
    }

    protected virtual void DeathAnimation()
    {
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