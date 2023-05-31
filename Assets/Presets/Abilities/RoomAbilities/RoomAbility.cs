using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomAbility : Ability
{
    public virtual void PartyEntered(Party party) {}
    public virtual void OnMonsterDied(Fighter f) {} // Maybe just change this to Monster?
    public virtual void OnHeroDied(Fighter f) {}
    public virtual float GetDamageMultiplier(Fighter f) {return 0f;}
}
