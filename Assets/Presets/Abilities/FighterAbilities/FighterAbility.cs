using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FighterAbility : Ability
{
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
    
    public virtual bool CanAddMonster(MonsterBase m, Room r) {return true;}
    public virtual float SelfOngoingDamage(Fighter f) {return 0f;}
    public virtual float AllyOngoingDamage(Fighter f, Fighter ally) {return 0f;}
    public virtual float SelfOngoingMaxHealth(Fighter f) {return 0f;}
    public virtual float AllyOngoingMaxHealth(Fighter f, Fighter ally) {return 0f;}
    public virtual bool ModifiesTargets() {return false;}
    public virtual List<Fighter> DecideTargets(List<Fighter> fighters) {return new List<Fighter>();}
}
