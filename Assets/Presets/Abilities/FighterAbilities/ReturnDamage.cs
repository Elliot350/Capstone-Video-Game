using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReturnDamage", menuName = "Abilities/Fighter/Return Damage")]
public class ReturnDamage : Ability
{
    [SerializeField] private float returnDamage;

    public override void OnTakenDamage(Damage attack)
    {
        attack.source.TakeDamage(new Damage(attack.target, attack.source, returnDamage));
    }

    public override string GetDescription()
    {
        return string.Format(description, returnDamage);
    }
}
