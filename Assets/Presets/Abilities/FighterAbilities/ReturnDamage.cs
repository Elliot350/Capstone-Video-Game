using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReturnDamage", menuName = "Abilities/Fighter/Return Damage")]
public class ReturnDamage : FighterAbility
{
    [SerializeField] private float returnDamage;

    public override void OnTakenDamage(Damage attack)
    {
        if (attack.source == null)
            return;
        // attack.source.TakeDamage(new Damage(attack.target, attack.source, returnDamage));
        FightManager.GetInstance().AddAction(new TakeDamage(new Damage(attack.target, attack.source, returnDamage)));
    }

    public override string GetDescription()
    {
        return string.Format(description, returnDamage);
    }
}
