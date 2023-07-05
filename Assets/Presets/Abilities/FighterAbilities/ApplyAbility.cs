using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplyAbility", menuName = "Abilities/Fighter/Apply Ability")]
public class ApplyAbility : FighterAbility
{
    [SerializeField] private FighterAbility ability;

    public override void OnAttack(Damage attack)
    {
        FightManager.GetInstance().AddAction(new AddAbility(attack.Target, ability));
    }

    public override string GetDescription()
    {
        return string.Format(description, ability.GetName(), ability.GetAbility());
    }
}
