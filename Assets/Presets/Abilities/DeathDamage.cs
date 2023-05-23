using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeathDamage", menuName = "Abilities/Death Damage")]
public class DeathDamage : Ability
{
    [SerializeField] private float deathDamage;

    public override void OnDeath(Damage attack)
    {
        attack.source.TakeDamage(new Damage(attack.target, attack.source, deathDamage));
    }

    public override string GetDescription()
    {
        return string.Format(description, deathDamage);
    }
}
