using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LessDamage", menuName = "Abilities/Fighter/Less Damage")]
public class LessDamage : FighterAbility
{
    [SerializeField] private int length;
    [SerializeField] private float damageReduction;

    public override void OnAttack(Damage attack)
    {
        length--;
        attack.DamageModifier -= damageReduction;
        if (length <= 0) FightManager.GetInstance().AddAction(new RemoveAbility(attack.Source, this));
    }

    public override string GetDescription()
    {
        return string.Format(description, length, damageReduction);
    }
}
