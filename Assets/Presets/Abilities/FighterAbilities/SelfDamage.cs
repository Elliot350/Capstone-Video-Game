using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SelfDamage", menuName = "Abilities/Figher/Self Damage")]
public class SelfDamage : FighterAbility
{
    [SerializeField] private float damage;
    [SerializeField] private bool removeAbility;

    public override void OnAttack(Damage attack)
    {
        Damage selfDamage = new Damage(null, attack.Source, damage);
        FightManager.GetInstance().AddAction(new TakeDamage(selfDamage));
        if (removeAbility)
        {
            FightManager.GetInstance().AddAction(new RemoveAbility(attack.Source, this));
        }
    }

    public override string GetDescription()
    {
        return string.Format(description, damage, removeAbility);
    }
}
