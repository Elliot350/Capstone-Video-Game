using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FighterAbility : Ability
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
        HERO_SUMMONED
    }

    [SerializeField] protected string animationTrigger;

    // Do I need this one?
    public virtual void OnAdded(Fighter f) {}
    public virtual void OnAttack(Damage attack) {}
    public virtual void OnTakenDamage(Damage attack) {}
    public virtual void OnDeath(Damage attack) {}
    public virtual void OnHeal(Fighter f) {}
    public virtual void OnFighterDied(Fighter f, Fighter dead) {}
    public virtual void TurnStart(Fighter f) {}
    public virtual void TurnEnd(Fighter f) {}
    public virtual void BattleStart(Fighter f) {}
    public virtual void BattleEnd(Fighter f) {}
    public virtual void FighterSummoned(Fighter f, Fighter newFighter) {}
    
    public virtual bool CanAddMonster(MonsterBase m, Room r) {return true;}
    public virtual void CalculateDamage(Fighter f) {}
    public virtual void CalculateMaxHealth(Fighter f) {}
    public virtual bool ModifiesTargets() {return false;}
    public virtual List<Fighter> DecideTargets(List<Fighter> fighters) {return new List<Fighter>();}
}