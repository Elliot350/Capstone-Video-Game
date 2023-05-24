using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomAbility : ScriptableObject
{
    [SerializeField] protected string abilityName;
    [SerializeField] protected string description;

    public virtual void PartyEntered(Party party) {}
    public virtual void OnMonsterDied(Fighter f) {} // Maybe just change this to Monster?
    public virtual void OnHeroDied(Fighter f) {}
    public virtual float GetDamageMultiplier(Fighter f) {return 0f;}

    public string GetName() {return abilityName;}
    public virtual string GetDescription() {return string.Format(description);}
    public virtual string Format() {return $"[{GetName()}] - {GetDescription()}";}
}
