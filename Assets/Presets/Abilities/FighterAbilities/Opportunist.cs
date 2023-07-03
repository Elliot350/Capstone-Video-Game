using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Opportunist", menuName = "Abilities/Fighter/Opportunist")]
public class Opportunist : FighterAbility
{
    public override void OnFighterDied(Fighter f, Fighter dead)
    {
        // Maybe change this to taking a whole turn?
        FightManager.GetInstance().AddAction(new GetTargets(f));
    }

    public override string GetDescription()
    {
        return string.Format(description);
    }
}
