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
        attack.SetDamage(0);
        FightManager.GetInstance().AddAction(new RemoveAbility(attack.GetSource(), this));
        FightManager.GetInstance().AddAction(new ContinueAnimation(attack.GetSource(), "FrozenDone", effect));
    }

    public override string GetDescription()
    {
        return description;
    }
}
