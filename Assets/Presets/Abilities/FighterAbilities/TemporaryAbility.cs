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

    public override void OnEndBattle(Fighter f)
    {
        ability.OnEndBattle(f);
        base.OnEndBattle(f);
    }

    public override void OnStartBattle(Fighter f)
    {
        ability.OnStartBattle(f);
        base.OnStartBattle(f);
    }

    public override void CalculateStats(Fighter f)
    {
        ability.CalculateStats(f);
        base.CalculateStats(f);
    }

    public override bool CanAddMonster(MonsterBase m, Room r)
    {
        return ability.CanAddMonster(m, r);
    }

    public override List<Fighter> DecideTargets(List<Fighter> fighters)
    {
        return ability.DecideTargets(fighters);
    }

    public override void OnFighterSummoned(Fighter f, Fighter newFighter)
    {
        ability.OnFighterSummoned(f, newFighter);
        base.OnFighterSummoned(f, newFighter);
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

    public override void OnEndTurn(Fighter f)
    {
        ability.OnEndTurn(f);
        base.OnEndTurn(f);
        if (countdown > 0) countdown--;
        if (countdown == 0) Activate(f);
    }

    public override void OnStartTurn(Fighter f)
    {
        ability.OnStartTurn(f);
        base.OnStartTurn(f);
    }
}
