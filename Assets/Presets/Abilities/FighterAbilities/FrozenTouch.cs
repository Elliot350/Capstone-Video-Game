using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Should change this to AddAbility instead
[CreateAssetMenu(fileName = "FrozenTouch", menuName = "Abilities/Fighter/Frozen Touch")]
public class FrozenTouch : Ability
{
    [SerializeField] private Ability ability;

    public override void OnAttack(Damage attack)
    {
        attack.target.AddAbility(ability);
    }

    public override string GetDescription()
    {
        return string.Format(description, ability.GetName(), ability.GetDescription());
    }
}
