using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeathDamage", menuName = "Abilities/Fighter/Death Damage")]
public class DeathDamage : FighterAbility
{
    [SerializeField] private float deathDamage;

    public override void OnDeath(Damage attack)
    {
        if (attack.source == null)
            return;
        attack.source.AddAction(new TakeDamage(new Damage(attack.target, attack.source, deathDamage)));
    }

    public override string GetDescription()
    {
        return string.Format(description, deathDamage);
    }
}
