using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    [SerializeField] protected string abilityName;
    [SerializeField] protected string description;



    public string GetName() {return abilityName;}
    public virtual string GetDescription() {return string.Format(description);}
    public virtual string GetAbility() {return $"[{GetName()}] - ({GetDescription()})";}
}
