using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReducedDamage", menuName = "Abilities/Fighter/Reduced Damage")]
public class ReducedDamage : FighterAbility
{
    [SerializeField] private float chance;
    [SerializeField] private float reduction;

    public override void OnTakenDamage(Damage attack)
    {
        Debug.Log($"Should be taking {attack.calculatedDamage}");
        if (Random.Range(0f, 1f) <= chance)
            // attack.DamageModifier -= reduction * attack.calculatedDamage;
            attack.baseDamage -= reduction * attack.calculatedDamage;
        Debug.Log($"Actually taking {attack.calculatedDamage}");
    }

    public override string GetDescription()
    {
        return string.Format(description, chance * 100, reduction * 100);
    }
}
