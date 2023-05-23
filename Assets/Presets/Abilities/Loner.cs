using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loner", menuName = "Abilities/Loner")]
public class Loner : Ability
{
    [SerializeField] private float damagePerSpace;

    public override float GetDamageMultiplier(Fighter f)
    {
        return (f.GetRoom().monsterCapacity - f.GetRoom().monsters.Count) * damagePerSpace;
    }

    public override string GetDescription()
    {
        return string.Format(description, damagePerSpace * 100);
    }
}
