using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class FightAction
{
    public Fighter fighter { get; private set; }
    protected float waitTime;

    public FightAction(Fighter fighter)
    {
        this.fighter = fighter;
        waitTime = 1f;
    }

    public abstract void Do();
    public virtual bool IsValid() { return fighter != null; }
    public virtual bool NeedsCalculation() { return true; }
    protected void AddAction(FightAction a) { FightManager.GetInstance().AddAction(a); }
    public float GetWaitTime() { return waitTime; }
}

public class Turn : FightAction
{
    public Turn(Fighter fighter) : base(fighter) { waitTime = 0f; }

    public override void Do()
    {
        AddAction(new StartTurn(fighter));
        AddAction(new GetTargets(fighter));
        AddAction(new EndTurn(fighter));
    }

    public override bool IsValid()
    {
        return base.IsValid() && !fighter.isDead;
    }
}

public class StartTurn : FightAction
{
    public StartTurn(Fighter fighter) : base(fighter) { waitTime = 0f; }
    public override void Do() { fighter.StartTurn(); }
}

public class GetTargets : FightAction
{
    public GetTargets(Fighter fighter) : base(fighter) { waitTime = 0f; }

    public override void Do()
    {
        List<Fighter> enemies = FightManager.GetInstance().GetEnemies(fighter);
        if (!FightManager.GetInstance().GetFighters().Contains(fighter) || enemies.Count == 0)
            return;
        foreach (Fighter f in fighter.GetTargets(enemies))
            AddAction(new Attack(fighter, f));
    }

    public override bool IsValid()
    {
        return base.IsValid() && !fighter.isDead && FightManager.GetInstance().GetEnemies(fighter).Count > 0;
    }
}

public class EndTurn : FightAction
{
    public EndTurn(Fighter fighter) : base(fighter) { waitTime = 0f; }
    public override void Do() { fighter.EndTurn(); }

    public override bool IsValid()
    {
        return base.IsValid() && !fighter.isDead;
    }
}


public class Attack : FightAction
{
    private Fighter target;

    public Attack(Fighter source, Fighter target) : base(source)
    {
        this.target = target;
    }

    public override void Do()
    {
        float attackDamage = fighter.GetDamage();
        Damage attack = new Damage(fighter, target, attackDamage);
        foreach (FighterAbility a in fighter.GetAbilities())
            a.OnAttack(attack);
        fighter.AttackAnimation();
        AddAction(new TakeDamage(attack));
    }

    public override bool IsValid()
    {
        return base.IsValid() && !fighter.isDead && target != null && !target.isDead;
    }
}

public class TakeDamage : FightAction
{
    private Damage attack;

    public TakeDamage(Damage attack) : base(attack.target) { this.attack = attack; }

    public override void Do()
    {
        if (FightManager.GetInstance().GetDead().Contains(attack.target)) return;
        fighter.TakeDamage(attack);
        if (fighter.GetHealth() <= 0)
            AddAction(new Die(fighter, attack));
    }

    public override bool IsValid()
    {
        return base.IsValid() && !attack.target.isDead && attack.target.GetHealth() >= 0;
    }
}

public class Die : FightAction
{
    private Damage attack;

    public Die(Fighter fighter) : this(fighter, new Damage(fighter, 0)) { }
    public Die(Fighter fighter, Damage attack) : base(fighter) { this.attack = attack; }

    public override void Do()
    {
        fighter.Die(attack);
        FightManager.GetInstance().FighterDied(fighter);
    }

    public override bool IsValid()
    {
        return base.IsValid() && !fighter.isDead;
    }
}

public class Heal : FightAction
{
    private float amount;

    public Heal(Fighter fighter, float amount) : base(fighter) { this.amount = amount; }
    
    public override void Do() 
    {
        if (fighter.GetHealth() >= fighter.GetMaxHealth()) waitTime = 0f; 
        fighter.Heal(amount); 
    }
    
    public override bool IsValid() { return base.IsValid() && !fighter.isDead; }
}

public class BattleStart : FightAction
{
    public BattleStart(Fighter fighter) : base(fighter) { waitTime = 0f; }
    public override void Do() { fighter.StartBattle(); }
    public override bool IsValid() { return base.IsValid() && !fighter.isDead; }
}

public class BattleEnd : FightAction
{
    public BattleEnd(Fighter fighter) : base(fighter) { waitTime = 0f; }
    public override void Do() { fighter.EndBattle(); }
    public override bool IsValid() { return base.IsValid() && !fighter.isDead; }
}

public class RemoveAbility : FightAction
{
    private FighterAbility ability;

    public RemoveAbility(Fighter fighter, FighterAbility ability) : base(fighter)
    {
        this.ability = ability;
        waitTime = 0f;
    }

    public override void Do()
    {
        if (!fighter.GetAbilities().Contains(ability)) return;
        foreach (FighterAbility a in fighter.GetAbilities())
            if (!a.CanRemoveAbility(ability)) return;
        fighter.GetAbilities().Remove(ability);
    }

    public override bool IsValid()
    {
        return base.IsValid() && fighter.GetAbilities().Contains(ability);
    }
}

public class AddAbility : FightAction
{
    private FighterAbility ability;

    public AddAbility(Fighter fighter, FighterAbility ability) : base(fighter)
    {
        this.ability = ability;
        waitTime = 0f;
    }

    public override void Do()
    {
        if (!fighter.GetAbilities().Contains(ability))
            fighter.GetAbilities().Add(GameObject.Instantiate(ability));
        ability.OnAdded(fighter);
    }

    public override bool IsValid()
    {
        // Might want to change this if I want fighters to have duplicate abilities
        return base.IsValid() && !fighter.GetAbilities().Contains(ability);
    }
}

public class PlayAnimation : FightAction
{
    string animationName;

    public PlayAnimation(Fighter fighter, string animationName) : base(fighter)
    {
        this.animationName = animationName;
        waitTime = 0f;
    }

    public override void Do() { fighter.PlayEffect(animationName); }
    public override bool NeedsCalculation() { return false; }
}

public class ContinueAnimation : FightAction
{
    string animationName;
    Effect effect;

    public ContinueAnimation(Fighter fighter, string animationName) : base(fighter)
    {
        this.animationName = animationName;
    }

    public ContinueAnimation(Fighter fighter, string animationName, Effect effect) : base(fighter)
    {
        this.animationName = animationName;
        this.effect = effect;
        waitTime = 0f;
    }

    public override void Do()
    {
        if (effect == null)
        {
            if (fighter.GetEffect(animationName) != null) fighter.PlayEffect(animationName, fighter.GetEffect(animationName));
        }
        else
        {
            fighter.PlayEffect(animationName, effect);
        }
    }

    public override bool NeedsCalculation() { return false; }
}

public class Morph : FightAction
{
    FighterBase fighterBase;

    public Morph(Fighter fighter, FighterBase fighterBase) : base(fighter) { this.fighterBase = fighterBase; }
    public override void Do() { fighter.SetBase(fighterBase); }
}

public class Summon : FightAction
{
    FighterBase summonFighterBase;
    List<FighterAbility> additionalAbilities;
    int position;

    public Summon(Fighter fighter, FighterBase summon, List<FighterAbility> abilities, int position) : base(fighter)
    {
        this.summonFighterBase = summon;
        this.additionalAbilities = abilities;
        this.position = position;
    }
    public Summon(Fighter fighter, FighterBase summon) : this(fighter, summon, new List<FighterAbility>(), -1) { }
    public Summon(Fighter fighter, FighterBase summon, int position) : this(fighter, summon, new List<FighterAbility>(), position) { }
    public Summon(Fighter fighter, FighterBase summon, List<FighterAbility> abilities) : this(fighter, summon, abilities, -1) { }

    public override void Do()
    {
        Debug.Log($"Summoning {summonFighterBase}, {summonFighterBase.GetName()}");
        Fighter summonedFighter = FightManager.GetInstance().SummonFighter(summonFighterBase, position);
        summonedFighter.SummonedAnimation();
        foreach (FighterAbility fa in additionalAbilities)
            AddAction(new AddAbility(summonedFighter, fa));
        AddAction(new BattleStart(summonedFighter));
    }
}

public class BuffFighter : FightAction
{
    private float healthAmount;
    private float damageAmount;

    public BuffFighter(Fighter fighter, float healthAmount, float damageAmount) : base(fighter)
    {
        this.healthAmount = healthAmount;
        this.damageAmount = damageAmount;
    }

    public override void Do()
    {
        fighter.IncreaseMaxHealth(healthAmount);
        fighter.IncreaseDamage(damageAmount);
    }
}

public class Revive : FightAction
{
    private float healAmount;

    // TODO: move this into Fighter?
    public Revive(Fighter fighter) : this(fighter, fighter.GetMaxHealth()) { }
    public Revive(Fighter fighter, float health) : base(fighter)
    {
        healAmount = health;
    }

    public override void Do()
    {
        FightManager manager = FightManager.GetInstance();
        if (!manager.GetDead().Contains(fighter))
            return;

        manager.GetDead().Remove(fighter);
        manager.GetFighters().Add(fighter);
        if (fighter.isMonster)
        {
            manager.GetMonsters().Add(fighter);
            fighter.transform.SetParent(manager.GetMonsterHolder().transform);
        }
        else
        {
            manager.GetHeroes().Add(fighter);
            fighter.transform.SetParent(manager.GetHeroHolder().transform);
        }
        fighter.isDead = false;
        fighter.ReviveAnimation();
        AddAction(new Heal(fighter, fighter.GetMaxHealth() / 2));
        AddAction(new BattleStart(fighter));
    }

    public override bool IsValid()
    {
        return base.IsValid() && fighter.isDead;
    }
}

public class Banish : FightAction
{
    public Banish(Fighter fighter) : base(fighter) { }

    public override void Do()
    {
        FightManager manager = FightManager.GetInstance();

        manager.GetFighters().Remove(fighter);
        manager.GetTeam(fighter).Remove(fighter);
        MonoBehaviour.Destroy(fighter.gameObject);
    }
}

public class Move : FightAction
{
    int newPosition;

    public Move(Fighter fighter, int position) : base(fighter)
    {
        this.newPosition = position;
    }

    public override void Do()
    {
        if (FightManager.GetInstance().GetTeam(fighter).IndexOf(fighter) == newPosition) waitTime = 0f;
        FightManager.GetInstance().MoveFighter(fighter, newPosition);
    }

    public override bool IsValid()
    {
        return base.IsValid() && !fighter.isDead;
    }
}

public class Delay : FightAction
{
    public Delay(float delayTime, Fighter fighter) : base(fighter) { waitTime = delayTime; }
    public Delay(float delayTime) : base(null) { waitTime = delayTime; }

    public override void Do() { }

    public override bool IsValid() { return true; }
    public override bool NeedsCalculation() { return false; }
}

public class ConditionalAction : FightAction
{
    private Func<bool> check;
    private FightAction action;

    public ConditionalAction(Func<bool> check, FightAction action) : base(action.fighter)
    {
        this.check = check;
        this.action = action;
    }

    public override void Do()
    {
        if (check()) action.Do();
    }

    public override bool IsValid() { return action.IsValid(); }
    public override bool NeedsCalculation() { return action.NeedsCalculation(); }
}

public class TestAction : FightAction
{
    Action<Fighter> action;

    public TestAction(Fighter fighter, Action<Fighter> action) : base(fighter) { this.action = action; }

    public override void Do()
    {
        action(fighter);
    }

    public override bool IsValid()
    {
        return true;
    }
}