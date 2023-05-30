using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Frozen", menuName = "Abilities/Fighter/Frozen")]
public class Frozen : Ability
{
    public override void OnAttack(Damage attack)
    {
        attack.damage = 0;
        attack.source.RemoveAbility(this);
    }
}
