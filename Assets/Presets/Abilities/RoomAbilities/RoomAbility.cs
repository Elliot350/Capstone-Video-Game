using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomAbility : Ability
{
    protected enum TargetType {
        MONSTER,
        HERO,
        BOTH
    }

    public virtual void Periodic() {}
    public virtual void RoomBuilt(Room r) {}
    public virtual void BattleStart(Room r, List<Fighter> monsters, List<Fighter> heroes) {}
    public virtual void BattleEnd(Room r, List<Fighter> monsters, List<Fighter> heroes) {}
    public virtual void OnFighterSummoned(Room r, Fighter f) {} 
    public virtual void OnFighterDied(Room r, Fighter f) {}
    public virtual void PartyWon(Party p) {}
    public virtual void CalculateDamage(Room r, List<Fighter> monsters, List<Fighter> heroes) {}
    public virtual void CalculateMaxHealth(Room r, List<Fighter> monsters, List<Fighter> heroes) {}
    public virtual void CalculateStats(Room r, List<Fighter> monsters, List<Fighter> heroes) {}
    public virtual bool CanAddMonster(Room r, MonsterBase monster) {return true;}
    public virtual bool CanAddTrap(Room r, TrapBase trap) {return true;}
}
