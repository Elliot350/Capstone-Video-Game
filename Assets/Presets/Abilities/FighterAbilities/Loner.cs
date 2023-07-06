using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Loner", menuName = "Abilities/Fighter/Loner")]
public class Loner : FighterAbility
{
    [SerializeField] private float damagePerSpace;

    public override void CalculateDamage(Fighter f)
    {
        float damageIncrease = (f.GetRoom().GetMonsterCapacity() - FightManager.GetInstance().GetAllies(f).Count) * damagePerSpace;
        f.IncreaseDamageModifier(damageIncrease);
    }

    public override string GetDescription()
    {
        return string.Format(description, damagePerSpace * 100);
    }
}
