using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ethereal", menuName = "Abilities/Fighter/Ethereal")]
public class Ethereal : FighterAbility
{
    public override void BattleEnd(Fighter f)
    {
        FightManager.GetInstance().AddAction(new Die(f));
    }

    public override string GetDescription()
    {
        return string.Format(description);
    }
}
