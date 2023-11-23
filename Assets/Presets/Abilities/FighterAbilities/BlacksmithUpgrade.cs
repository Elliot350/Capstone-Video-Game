using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BlacksmithUpgrade", menuName = "Abilities/Fighter/Upgraded Weapons")]
public class BlacksmithUpgrade : FighterAbility
{
    [SerializeField] private float startingDamageBuff;
    [SerializeField] private float decayValue;

    public override void CalculateDamage(Fighter f)
    {
        f.IncreaseDamageModifier(startingDamageBuff);
    }

    public override void TurnEnd(Fighter f)
    {
        startingDamageBuff -= decayValue;
        if (startingDamageBuff <= 0) FightManager.GetInstance().AddAction(new RemoveAbility(f, this));
    }

    public override string GetDescription()
    {
        return string.Format(description, startingDamageBuff, decayValue);
    }
}
