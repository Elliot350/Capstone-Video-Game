using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FighterAbility : Ability
{
    [SerializeField] protected Animation animation;

    public virtual void OnTakenDamage(Damage attack) {}
    public virtual void OnHeal(Fighter f) {}
    public virtual void OnAttack(Damage attack) {}
    public virtual void OnDeath(Damage attack) {}
    public virtual void OnHeroDied(Fighter f, Fighter dead) {OnFighterDied(f, dead);}
    public virtual void OnMonsterDied(Fighter f, Fighter dead) {OnFighterDied(f, dead);}
    protected virtual void OnFighterDied(Fighter f, Fighter dead) {}
    public virtual void OnBattleStarted(Fighter f) {}
    public virtual void OnBattleFinished(Fighter f) {}
    public virtual float GetDamageMultiplier(Fighter f) {return 0f;}

    public virtual List<Fighter> DecideTargets(List<Fighter> fighters) {return new List<Fighter>();}
}
