using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vampirism", menuName = "Abilities/Fighter/Vampirism")]
public class Vampirism : FighterAbility
{
    [SerializeField] private float healAmount;

    public override void OnAttack(Damage attack)
    {
        attack.source.Heal(healAmount);
    }

    public override string GetDescription()
    {
        return string.Format(description, healAmount);
    }
}
