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
        if (attack.source == null || FightManager.GetInstance().GetDead().Contains(attack.source))
            return;
        if (!animationTrigger.Equals("")) attack.target.PlayEffect(animationTrigger);
        FightManager.GetInstance().AddAction(new TakeDamage(new Damage(attack.target, attack.source, returnDamage)));
    }

    public override string GetDescription()
    {
        return string.Format(description, returnDamage);
    }
}
