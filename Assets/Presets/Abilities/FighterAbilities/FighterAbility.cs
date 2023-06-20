using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FighterAbility : Ability
{
    [SerializeField] protected string animationTrigger;

    public virtual void OnAdded(Fighter f) {}
    public virtual void OnTakenDamage(Damage attack) {}
    public virtual void OnHeal(Fighter f) {}
    public virtual void OnAttack(Damage attack) {}
    public virtual void OnDeath(Damage attack) {}
    public virtual void OnHeroDied(Fighter f, Fighter dead) {OnFighterDied(f, dead);}
    public virtual void OnMonsterDied(Fighter f, Fighter dead) {OnFighterDied(f, dead);}
    public virtual void OnFighterDied(Fighter f, Fighter dead) {}
    public virtual void OnBattleStarted(Fighter f) {}
    public virtual void OnBattleFinished(Fighter f) {}
    public virtual bool CanAddMonster(MonsterBase m, Room r) {return true;}
    public virtual float GetDamageMultiplier(Fighter f) {return 0f;}

    public virtual bool ModifiesTargets() {return false;}
    public virtual List<Fighter> DecideTargets(List<Fighter> fighters) {return new List<Fighter>();}
}
