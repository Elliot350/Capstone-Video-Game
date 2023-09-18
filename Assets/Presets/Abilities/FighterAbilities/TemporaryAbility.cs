using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TemporaryAbility", menuName = "Abilities/Fighter/Temporary Ability")]
public class TemporaryAbility : TriggeredFighterAbility
{
    [SerializeField] private FighterAbility temporaryAbility;
    [SerializeField] private int countdown;
    private FighterAbility ability;

    public override void OnAdded(Fighter f)
    {
        ability = Instantiate(temporaryAbility);
    }

    protected override void Activate(Fighter target)
    {
        FightManager.GetInstance().AddAction(new RemoveAbility(target, this));
    }

    public override void BattleEnd(Fighter f)
    {
        ability.BattleEnd(f);
        base.BattleEnd(f);
    }

    public override void BattleStart(Fighter f)
    {
        ability.BattleStart(f);
        base.BattleStart(f);
    }

    public override void CalculateDamage(Fighter f)
    {
        ability.CalculateDamage(f);
        base.CalculateDamage(f);
    }

    public override void CalculateMaxHealth(Fighter f)
    {
        ability.CalculateMaxHealth(f);
        base.CalculateMaxHealth(f);
    }

    public override bool CanAddMonster(MonsterBase m, Room r)
    {
        return ability.CanAddMonster(m, r);
    }

    public override List<Fighter> DecideTargets(List<Fighter> fighters)
    {
        return ability.DecideTargets(fighters);
    }

    public override void FighterSummoned(Fighter f, Fighter newFighter)
    {
        ability.FighterSummoned(f, newFighter);
        base.FighterSummoned(f, newFighter);
    }

    public override string GetAbility()
    {
        return base.GetAbility();
    }

    public override string GetDescription()
    {
        return string.Format(description, temporaryAbility.GetDescription());
    }

    public override int ModifiesTargets()
    {
        return ability.ModifiesTargets();
    }

    public override void OnAttack(Damage attack)
    {
        ability.OnAttack(attack);
        base.OnAttack(attack);
    }

    public override void OnDeath(Damage attack)
    {
        ability.OnDeath(attack);
        base.OnDeath(attack);
    }

    public override void OnFighterDied(Fighter f, Fighter dead)
    {
        ability.OnFighterDied(f, dead);
        base.OnFighterDied(f, dead);
    }

    public override void OnHeal(Fighter f)
    {
        ability.OnHeal(f);
        base.OnHeal(f);
    }

    public override void OnTakenDamage(Damage attack)
    {
        ability.OnTakenDamage(attack);
        base.OnTakenDamage(attack);
    }

    public override void TurnEnd(Fighter f)
    {
        ability.TurnEnd(f);
        base.TurnEnd(f);
        if (countdown > 0) countdown--;
        if (countdown == 0) Activate(f);
    }

    public override void TurnStart(Fighter f)
    {
        ability.TurnStart(f);
        base.TurnStart(f);
    }
}
