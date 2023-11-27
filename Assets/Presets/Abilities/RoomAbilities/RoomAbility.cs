using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomAbility : Ability
{
    public virtual void Periodic() {}
    public virtual void RoomBuilt(Room r) {}
    public virtual void PartyEntered(Party party) {}
    public virtual void BattleStart(Room r, List<Fighter> monsters, List<Fighter> heroes) {}
    public virtual void BattleEnd(Room r, List<Fighter> monsters, List<Fighter> heroes) {}
    public virtual void OnMonsterAdded(MonsterBase mb) {} 
    public virtual void FighterSummoned(Room r, Fighter f) {} 
    public virtual void OnFighterDied(Room r, Fighter f) {}
    public virtual void PartyWon(Party p) {}
    public virtual void CalculateDamage(Fighter f) {}
    public virtual void CalculateMaxHealth(Fighter f) {}
    public virtual bool CanAddMonster(Room r, MonsterBase monster) {return true;}
    public virtual bool CanAddTrap(Room r, TrapBase trap) {return true;}
}
