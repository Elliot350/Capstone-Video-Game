using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragile : TriggeredFighterAbility
{
    protected override void Activate(Fighter target)
    {
        FightManager.GetInstance().AddAction(new Die(target));
    }

    public override string GetDescription()
    {
        return string.Format(description);
    }
}
