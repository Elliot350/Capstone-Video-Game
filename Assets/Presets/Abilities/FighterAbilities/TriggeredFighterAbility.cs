using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggeredFighterAbility : FighterAbility
{
    [SerializeField] protected List<Trigger> triggers;

    protected abstract void Activate(Fighter target);

    public override void BattleStart(Fighter f) {if (triggers.Contains(Trigger.START_BATTLE)) Activate(f);}
    public override void BattleEnd(Fighter f) {if (triggers.Contains(Trigger.END_BATTLE)) Activate(f);}
    public override void TurnStart(Fighter f) {if (triggers.Contains(Trigger.START_TURN)) Activate(f);}
    public override void TurnEnd(Fighter f) {if (triggers.Contains(Trigger.END_TURN)) Activate(f);}
    public override void OnAttack(Damage attack) {if (triggers.Contains(Trigger.ATTACK)) Activate(attack.Source);}
    public override void OnTakenDamage(Damage attack) {if (triggers.Contains(Trigger.DAMAGED)) Activate(attack.Target);}
    public override void OnDeath(Damage attack) {if (triggers.Contains(Trigger.DEATH)) Activate(attack.Target);}
    public override void OnFighterDied(Fighter f, Fighter dead) {if ((triggers.Contains(Trigger.MONSTER_DIED) && dead.IsMonster) || (triggers.Contains(Trigger.HERO_DIED) && !dead.IsMonster)) Activate(f);}
    public override void OnHeal(Fighter f) {if (triggers.Contains(Trigger.HEALED)) Activate(f);}
    public override void FighterSummoned(Fighter f, Fighter newFighter) {if ((triggers.Contains(Trigger.MONSTER_SUMMONED) && newFighter.IsMonster) || (triggers.Contains(Trigger.HERO_SUMMONED) && !newFighter.IsMonster)) Activate(f);}
}
