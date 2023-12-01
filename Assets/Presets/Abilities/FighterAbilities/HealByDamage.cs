using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealByDamage : FighterAbility
{
    [SerializeField] private float healMultiplier;

    public override void OnAttack(Damage attack)
    {
        List<Fighter> allies = FightManager.GetInstance().GetAllies(attack.source);
        FightManager.GetInstance().AddAction(new Heal(allies[Random.Range(0, allies.Count)], attack.calculatedDamage * healMultiplier));
    }

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }
}
