using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [SerializeField] protected string abilityName;
    [SerializeField] protected string description;

    public string GetName() {return abilityName;}
    public abstract string GetDescription();
    public virtual string GetAbility() {return $"[{GetName()}] - ({GetDescription()})";}
}
