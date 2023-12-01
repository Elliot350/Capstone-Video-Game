using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Frozen", menuName = "Abilities/Fighter/Frozen")]
public class Frozen : FighterAbility
{
    private Effect effect;

    public override void OnAdded(Fighter f)
    {
        effect = f.PlayEffect("Frozen");
    }

    public override void OnAttack(Damage attack)
    {
        attack.baseDamage = 0;
        FightManager.GetInstance().AddAction(new RemoveAbility(attack.source, this));
        FightManager.GetInstance().AddAction(new ContinueAnimation(attack.source, "FrozenDone", effect));
    }

    public override string GetDescription()
    {
        return description;
    }
}
