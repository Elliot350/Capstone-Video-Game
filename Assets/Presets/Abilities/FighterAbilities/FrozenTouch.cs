using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FrozenTouch", menuName = "Abilities/Fighter/Frozen Touch")]
public class FrozenTouch : FighterAbility
{
    [SerializeField] private FighterAbility ability;

    public override void OnAttack(Damage attack)
    {
        FightManager.GetInstance().AddAction(new AddAbility(attack.GetTarget(), ability));
    }

    public override string GetDescription()
    {
        return string.Format(description, ability.GetName(), ability.GetDescription());
    }
}
