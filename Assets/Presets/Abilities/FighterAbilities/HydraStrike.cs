using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HydraStrike", menuName = "Abilities/Fighter/Hydra Strike")]
public class HydraStrike : FighterAbility
{
    public override List<Fighter> DecideTargets(List<Fighter> fighters)
    {
        return fighters;
    }
}
