using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MutualDeath", menuName = "Abilities/Fighter/Mutual Death")]
public class MutualDeath : FighterAbility
{
    public override void OnDeath(Damage attack)
    {
        FightManager.GetInstance().AddAction(new Die(attack.Source, new Damage(attack.Target, attack.Source, 0)));
    }

    public override string GetDescription()
    {
        return string.Format(description);
    }
}
