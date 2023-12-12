using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FighterAbility : Ability
{
    protected const int NONE = 0;
    protected const int ADDS_ATTACKS = 1;
    protected const int PREVENTS_ATTACKS = 2;

    [SerializeField] protected string animationTrigger;

    // Do I need this one?
    public virtual void OnAdded(Fighter f) {}
    public virtual void OnAttack(Damage attack) {}
    public virtual void OnTakenDamage(Damage attack) {}
    public virtual void OnDeath(Damage attack) {}
    public virtual void OnHeal(Fighter f) {}
    public virtual void OnFighterDied(Fighter f, Fighter dead) {}
    public virtual void OnStartTurn(Fighter f) {}
    public virtual void OnEndTurn(Fighter f) {}
    public virtual void OnStartBattle(Fighter f) {}
    public virtual void OnEndBattle(Fighter f) {}
    public virtual void OnFighterSummoned(Fighter f, Fighter newFighter) {}
    public virtual void OnMoved(Fighter f) {}
    
    public virtual void CalculateStats(Fighter f) {} 
    public virtual bool CanAddMonster(MonsterBase m, Room r) {return true;}
    public virtual int ModifiesTargets() {return NONE;}
    public virtual List<Fighter> DecideTargets(List<Fighter> fighters) {return new List<Fighter>();}
}