using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Revival", menuName = "Abilities/Fighter/Revival")]
public class Revival : FighterAbility
{
    public override void OnDeath(Damage attack)
    {
        FightManager.GetInstance().AddAction(new Revive(attack.Target));
        FightManager.GetInstance().AddAction(new RemoveAbility(attack.Target, this));
    }

    public override string GetDescription()
    {
        return string.Format(description);
    }
}
