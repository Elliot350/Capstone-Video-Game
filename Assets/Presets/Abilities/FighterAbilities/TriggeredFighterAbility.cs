using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggeredFighterAbility : FighterAbility
{
    protected enum Trigger {
        START_BATTLE,
        END_BATTLE,
        START_TURN,
        END_TURN,
        ATTACK,
        DAMAGED,
        DEATH,
        MONSTER_DIED,
        HERO_DIED,
        HEALED,
        MONSTER_SUMMONED,
        HERO_SUMMONED,
        MOVED
    }

    [SerializeField] protected List<Trigger> triggers;

    protected abstract void Activate(Fighter self);

    public override void BattleStart(Fighter f) {if (triggers.Contains(Trigger.START_BATTLE)) Activate(f);}
    public override void BattleEnd(Fighter f) {if (triggers.Contains(Trigger.END_BATTLE)) Activate(f);}
    public override void TurnStart(Fighter f) {if (triggers.Contains(Trigger.START_TURN)) Activate(f);}
    public override void TurnEnd(Fighter f) {if (triggers.Contains(Trigger.END_TURN)) Activate(f);}
    public override void OnAttack(Damage attack) {if (triggers.Contains(Trigger.ATTACK)) Activate(attack.source);}
    public override void OnTakenDamage(Damage attack) {if (triggers.Contains(Trigger.DAMAGED)) Activate(attack.target);}
    public override void OnDeath(Damage attack) {if (triggers.Contains(Trigger.DEATH)) Activate(attack.target);}
    public override void OnFighterDied(Fighter f, Fighter dead) {if ((triggers.Contains(Trigger.MONSTER_DIED) && dead.isMonster) || (triggers.Contains(Trigger.HERO_DIED) && !dead.isMonster)) Activate(f);}
    public override void OnHeal(Fighter f) {if (triggers.Contains(Trigger.HEALED)) Activate(f);}
    public override void FighterSummoned(Fighter f, Fighter newFighter) {if ((triggers.Contains(Trigger.MONSTER_SUMMONED) && newFighter.isMonster) || (triggers.Contains(Trigger.HERO_SUMMONED) && !newFighter.isMonster)) Activate(f);}
    public override void OnMoved(Fighter f) {if (triggers.Contains(Trigger.MOVED)) Activate(f);}
}
