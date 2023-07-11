using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pacifist", menuName = "Abilities/Fighter/Pacifist")]
public class NoAttack : FighterAbility
{
    public override List<Fighter> DecideTargets(List<Fighter> fighters)
    {
        return new List<Fighter>();
    }

    public override bool ModifiesTargets() {return true;}

    public override string GetDescription()
    {
        return string.Format(description);
    }
}
