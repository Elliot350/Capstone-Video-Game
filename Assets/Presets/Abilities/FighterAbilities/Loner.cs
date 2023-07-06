using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loner", menuName = "Abilities/Fighter/Loner")]
public class Loner : FighterAbility
{
    [SerializeField] private float damagePerSpace;

    public override float CalculateSelfModifier(Fighter f)
    {
        return (f.GetRoom().GetMonsterCapacity() - FightManager.GetInstance().GetMonsters().Count) * damagePerSpace;
    }

    public override string GetDescription()
    {
        return string.Format(description, damagePerSpace * 100);
    }
}
