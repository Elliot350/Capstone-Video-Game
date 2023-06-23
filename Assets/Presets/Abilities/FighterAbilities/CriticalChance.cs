using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CriticalChance", menuName = "Abilities/Fighter/Critical Chance")]
public class CriticalChance : FighterAbility
{
    [SerializeField] protected float criticalChance;
    [SerializeField] protected float criticalMultiplier;

    public override void OnAttack(Damage attack)
    {
        if (Random.Range(0f, 1f) < criticalChance)
            attack.Multiplier += criticalMultiplier;
    }

    public override string GetDescription()
    {
        return string.Format(description, criticalChance * 100, criticalMultiplier * 100);
    }
}
