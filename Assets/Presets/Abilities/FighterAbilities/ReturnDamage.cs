using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ReturnDamage", menuName = "Abilities/Fighter/Return Damage")]
public class ReturnDamage : FighterAbility
{
    [SerializeField] private float returnDamage;

    public override void OnTakenDamage(Damage attack)
    {
        // If there isn't a source or the attacked is dead
        if (attack.Source == null || FightManager.GetInstance().GetDead().Contains(attack.Source))
            return;
        if (!animationTrigger.Equals("")) attack.Target.PlayEffect(animationTrigger);
        FightManager.GetInstance().AddAction(new TakeDamage(new Damage(attack.Target, attack.Source, returnDamage)));
    }

    public override string GetDescription()
    {
        return string.Format(description, returnDamage);
    }
}
