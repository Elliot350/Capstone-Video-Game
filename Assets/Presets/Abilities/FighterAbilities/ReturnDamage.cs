using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReturnDamage", menuName = "Abilities/Fighter/Return Damage")]
public class ReturnDamage : FighterAbility
{
    [SerializeField] private float returnDamage;

    public override void OnTakenDamage(Damage attack)
    {
        if (attack.GetSource() == null)
            return;
        attack.GetTarget().PlayEffect(animationTrigger);
        FightManager.GetInstance().AddAction(new TakeDamage(new Damage(attack.GetTarget(), attack.GetSource(), returnDamage)));
    }

    public override string GetDescription()
    {
        return string.Format(description, returnDamage);
    }
}
