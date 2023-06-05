using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HydraStrike", menuName = "Abilities/Fighter/Hydra Strike")]
public class HydraStrike : FighterAbility
{
    public override List<Fighter> DecideTargets(List<Fighter> fighters)
    {
        List<Fighter> targets = new List<Fighter>(fighters);
        targets.Reverse();
        return targets;
    }

    public override string GetDescription()
    {
        return description;
    }
}
