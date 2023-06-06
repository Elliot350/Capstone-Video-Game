using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoomAbility : Ability
{
    public virtual void RoomBuilt(Room r) {}
    public virtual void PartyEntered(Party party) {}
    public virtual void FightStarted(List<Fighter> monsters, List<Fighter> heroes) {}
    public virtual void OnMonsterAdded(MonsterBase mb) {} 
    public virtual void OnHeroDied(Fighter f) {OnFighterDied(f);}
    public virtual void OnMonsterDied(Fighter f) {OnFighterDied(f);}
    public virtual void OnFighterDied(Fighter f) {}
    public virtual void PartyWon(Party p) {}
    public virtual float GetDamageMultiplier(Fighter f) {return 0f;}
    public virtual bool CanAddMonster(MonsterBase monster) {return true;}
}
