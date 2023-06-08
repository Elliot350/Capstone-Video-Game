using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeathDamage", menuName = "Abilities/Fighter/Death Damage")]
public class DeathDamage : FighterAbility
{
    [SerializeField] private float deathDamage;

    public override void OnDeath(Damage attack)
    {
        if (attack.GetSource() == null)
            return;
        FightManager.GetInstance().AddAction(new TakeDamage(new Damage(attack.GetTarget(), attack.GetSource(), deathDamage)));
    }

    public override string GetDescription()
    {
        return string.Format(description, deathDamage);
    }
}
