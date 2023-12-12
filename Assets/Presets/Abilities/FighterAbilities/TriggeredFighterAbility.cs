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
        TAKEN_DAMAGE,
        DEATH,
        MONSTER_DIED,
        HERO_DIED,
        HEAL,
        MONSTER_SUMMONED,
        HERO_SUMMONED,
        MOVED
    }

    [SerializeField] protected List<Trigger> triggers;

    protected abstract void Activate(Fighter self);

    public override void OnStartBattle(Fighter f) {if (triggers.Contains(Trigger.START_BATTLE)) Activate(f);}
    public override void OnEndBattle(Fighter f) {if (triggers.Contains(Trigger.END_BATTLE)) Activate(f);}
    public override void OnStartTurn(Fighter f) {if (triggers.Contains(Trigger.START_TURN)) Activate(f);}
    public override void OnEndTurn(Fighter f) {if (triggers.Contains(Trigger.END_TURN)) Activate(f);}
    public override void OnAttack(Damage attack) {if (triggers.Contains(Trigger.ATTACK)) Activate(attack.source);}
    public override void OnTakenDamage(Damage attack) {if (triggers.Contains(Trigger.TAKEN_DAMAGE)) Activate(attack.target);}
    public override void OnDeath(Damage attack) {if (triggers.Contains(Trigger.DEATH)) Activate(attack.target);}
    public override void OnFighterDied(Fighter f, Fighter dead) {if ((triggers.Contains(Trigger.MONSTER_DIED) && dead.isMonster) || (triggers.Contains(Trigger.HERO_DIED) && !dead.isMonster)) Activate(f);}
    public override void OnHeal(Fighter f) {if (triggers.Contains(Trigger.HEAL)) Activate(f);}
    public override void OnFighterSummoned(Fighter f, Fighter newFighter) {if ((triggers.Contains(Trigger.MONSTER_SUMMONED) && newFighter.isMonster) || (triggers.Contains(Trigger.HERO_SUMMONED) && !newFighter.isMonster)) Activate(f);}
    public override void OnMoved(Fighter f) {if (triggers.Contains(Trigger.MOVED)) Activate(f);}
}
