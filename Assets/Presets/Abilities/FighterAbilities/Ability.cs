using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : ScriptableObject
{
    [SerializeField] protected string abilityName;
    [SerializeField] protected string description;

    public virtual void OnTakenDamage(Damage attack) {}
    public virtual void OnHeal(Fighter f) {}
    public virtual void OnAttack(Fighter f) {}
    public virtual void OnDeath(Damage attack) {}
    public virtual void OnBattleStarted(Fighter f) {}
    public virtual void OnBattleFinished(Fighter f) {}
    public virtual float GetDamageMultiplier(Fighter f) {return 0f;}

    public string GetName() {return abilityName;}
    public virtual string GetDescription() {return string.Format(description);}
    public virtual string Format() {return $"[{GetName()}] - {GetDescription()}";}
}
