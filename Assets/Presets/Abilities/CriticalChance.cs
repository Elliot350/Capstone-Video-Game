using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CriticalChance", menuName = "Abilities/Critical Chance")]
public class CriticalChance : Ability
{
    [SerializeField] protected float criticalChance;
    [SerializeField] protected float criticalMultiplier;

    public override float GetDamageMultiplier(Fighter f)
    {
        return Random.Range(0f, 1f) < criticalChance ? criticalMultiplier : 0;
    }

    public override string GetDescription()
    {
        return string.Format(description, criticalChance * 100, criticalMultiplier * 100);
    }
}
