using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomDamage", menuName = "Abilities/Trap/Random Damage")]
public class RandomDamage : TrapAbility
{
    [SerializeField] private float damage;

    public override void PartyEntered(Party party)
    {
        if (CheckChance())
            party.DamageHero(damage);
    }

    public override string GetDescription()
    {
        return string.Format(description, chance * 100, damage);
    }
}
