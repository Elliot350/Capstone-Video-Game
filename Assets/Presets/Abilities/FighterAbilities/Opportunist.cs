using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Opportunist", menuName = "Abilities/Fighter/Opportunist")]
public class Opportunist : FighterAbility
{
    public override void OnFighterDied(Fighter f, Fighter dead)
    {
        FightManager.GetInstance().AddAction(new GetTargets(f, FightManager.GetInstance().GetHeroes()));
    }

    public override string GetDescription()
    {
        return string.Format(description);
    }
}
