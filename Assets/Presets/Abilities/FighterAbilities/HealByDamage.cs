using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealByDamage : FighterAbility
{
    enum Target {
        SELF,
        RANDOM_ALLY
    }

    [SerializeField] private float healMultiplier;
    [SerializeField] private Target target;

    public override void OnAttack(Damage attack)
    {
        List<Fighter> allies = FightManager.GetInstance().GetAllies(attack.source);
        switch (target) {
            case Target.SELF:
                FightManager.GetInstance().AddAction(new Heal(attack.source, attack.calculatedDamage * healMultiplier));
                break;
            case Target.RANDOM_ALLY:
                FightManager.GetInstance().AddAction(new Heal(allies[Random.Range(0, allies.Count)], attack.calculatedDamage * healMultiplier));
                break;
        }
    }

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }
}
