using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveToBack", menuName = "Abilities/Fighter/Move To Back")]
public class MoveToBack : TriggeredFighterAbility
{
    protected override void Activate(Fighter self)
    {
        FightManager.GetInstance().AddAction(new Move(self, -1));
    }

    public override string GetDescription()
    {
        return string.Format(description);
    }
}
