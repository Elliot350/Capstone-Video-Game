using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeathDamage", menuName = "Abilities/Fighter/Death Damage")]
public class DeathDamage : FighterAbility
{
    [SerializeField] private float deathDamage;

    public override void OnDeath(Damage attack)
    {
        if (attack.Source == null)
            return;
        FightManager.GetInstance().AddAction(new TakeDamage(new Damage(attack.Target, attack.Source, deathDamage)));
    }

    public override string GetDescription()
    {
        return string.Format(description, deathDamage);
    }
}
