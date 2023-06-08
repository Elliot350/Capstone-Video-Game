using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Frozen", menuName = "Abilities/Fighter/Frozen")]
public class Frozen : FighterAbility
{
    public override void OnAttack(Damage attack)
    {
        attack.SetDamage(0);
        FightManager.GetInstance().AddAction(new RemoveAbility(attack.GetSource(), this));
    }

    public override string GetDescription()
    {
        return description;
    }
}
