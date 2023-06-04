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
    public bool alive = true;
    private bool busy;
    protected Fighter lastAttacker;
    protected Room room;
    protected FighterBase fighterBase;

    [SerializeField] private TextMeshProUGUI actionCount;

    private List<Action> actions = new List<Action>();
    private WaitForSeconds pause = new WaitForSeconds(1);

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

    // public virtual void TakeDamage(Damage attack)
    // {
    //     foreach (FighterAbility a in abilities)
    //         a.OnTakenDamage(attack);
    //     HurtAnimation();
    //     health -= attack.damage;
    //     SetHealthBar();
    //     if (health <= 0)
    //         Die(attack);
    // }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        SetHealthBar();
        HurtAnimation();
    }

    public virtual void Heal(float amount)
    {
        health += amount;
        SetHealthBar();
    }

    public IEnumerator DoActions()
    {
        busy = true;
        ShowActions();
        while (actions.Count > 0 && alive)
        {
            yield return pause;
            Debug.Log($"Doing action {actions[0]}");
            Action upcomingAction = actions[0];
            actions.RemoveAt(0);
            yield return StartCoroutine(upcomingAction.Do());
            ShowActions();
            
            if (!alive)
            {
                busy = false;
                DestroyGameObject();
                yield break;
            }
        }
        busy = false;
        Debug.Log("No more actions");
        yield break;
    }

    private void ShowActions()
    {
        string str = actions.Count.ToString() + ":\n";
        foreach (Action a in actions)
        {
            str += a + "\n";
        }
        actionCount.text = str;
    }

    public void AddAction(Action action)
    {
        // Debug.Log($"Adding action - {action}");
        if (actions.Count > 1)
            actions.Insert(0, action);
        else 
            actions.Add(action);
    }

    public void AddImportantAction(Action action)
    {
        // Debug.Log($"Adding ACTION - {action}");
        if (actions.Count > 1)
            actions.Insert(0, action);
        else 
            actions.Add(action);
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

    public float CalculateDamage()
    {
        return damage * CalculateDamageMultiplier();
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
        Debug.Log($"Destroying...");
        // StopAllCoroutines();
        Destroy(gameObject, 0.75f);
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

    public virtual void AttackAnimation()
    {
        animator.SetBool("Monster", isMonster);
        animator.SetTrigger("Attack");
    }

    public virtual void HurtAnimation()
    {
        animator.SetBool("Monster", isMonster);
        animator.SetTrigger("Hurt");
    }

    public virtual void DeathAnimation()
    {
        alive = false;
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
    public bool IsBusy() {return busy;}
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

// ---------- Actions ----------

public abstract class Action
{
    public Fighter fighter;

    public Action(Fighter fighter) 
    {this.fighter = fighter;}

    public abstract IEnumerator Do();
}

public class Attack : Action
{
    private Fighter source, target;

    public Attack(Fighter source, Fighter target) : base (source)
    {
        this.source = source;
        this.target = target;
    }

    public override IEnumerator Do()
    {
        float attackDamage = source.CalculateDamage();
        Damage attack = new Damage(source, target, attackDamage);
        foreach (FighterAbility a in source.GetAbilities())
            a.OnAttack(attack);
        source.AttackAnimation();
        target.AddAction(new TakeDamage(attack));
        yield return target.StartCoroutine(target.DoActions());
    }
}

public class TakeDamage : Action
{
    private Damage attack;

    public TakeDamage(Damage attack) : base(attack.target)
    {
        this.attack = attack;
    }

    public override IEnumerator Do()
    {
        foreach (FighterAbility a in fighter.GetAbilities())
            a.OnTakenDamage(attack);
        fighter.TakeDamage(attack.damage);
        if (fighter.GetHealth() <= 0)
            fighter.AddImportantAction(new Die(fighter, attack));
        yield break;
    }
}

public class Heal : Action
{
    private float amount;

    public Heal(Fighter fighter, float amount) : base(fighter)
    {
        this.amount = amount;
    }

    public override IEnumerator Do()
    {
        foreach (FighterAbility a in fighter.GetAbilities())
            a.OnHeal(fighter);
        fighter.Heal(amount);
        yield break;
    }
}

public class RemoveAbility : Action
{
    private FighterAbility ability;

    public RemoveAbility(Fighter fighter, FighterAbility ability) : base(fighter)
    {
        this.fighter = fighter;
        this.ability = ability;
    }

    public override IEnumerator Do()
    {
        if (fighter.GetAbilities().Contains(ability))
            fighter.GetAbilities().Remove(ability);
        yield break;
    }
}

public class AddAbility : Action
{
    private FighterAbility ability;

    public AddAbility(Fighter fighter, FighterAbility ability) : base(fighter)
    {
        this.fighter = fighter;
        this.ability = ability;
    }

    public override IEnumerator Do()
    {
        if (!fighter.GetAbilities().Contains(ability))
            fighter.GetAbilities().Add(ability);
        yield break;
    }
}

public class Die : Action
{
    private Damage attack;

    public Die(Fighter fighter, Damage attack) : base(fighter)
    {
        this.attack = attack;
    }

    public override IEnumerator Do()
    {
        foreach (FighterAbility a in fighter.GetAbilities())
            a.OnDeath(attack);
        FightManager.GetInstance().FighterDied(fighter);
        fighter.DeathAnimation();
        yield break;
    }
}

public class GetTargets : Action
{
    private List<Fighter> fighters;

    public GetTargets(Fighter fighter, List<Fighter> fighters) : base(fighter)
    {
        this.fighters = fighters;
    }

    public override IEnumerator Do()
    {
        List<Fighter> targets = new List<Fighter>();
        targets.Add(fighters[0]);
        foreach (FighterAbility a in fighter.GetAbilities())
        {
            if (a.DecideTargets(fighters).Count >= targets.Count)
                targets = a.DecideTargets(fighters);
        }
        foreach (Fighter f in targets)
            fighter.AddAction(new Attack(fighter, f));
        yield break;
    }
}

public class TriggerOtherFighter : Action
{
    private Fighter target;

    public TriggerOtherFighter(Fighter fighter, Fighter target) : base(fighter)
    {
        this.target = target;
    }

    public override IEnumerator Do()
    {
        yield return target.StartCoroutine(target.DoActions());
    }
}