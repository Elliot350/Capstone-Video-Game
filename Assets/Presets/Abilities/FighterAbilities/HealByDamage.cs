using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealByDamage : FighterAbility
{
    [SerializeField] private float healMultiplier;

    public override void OnAttack(Damage attack)
    {
        List<Fighter> allies = FightManager.GetInstance().GetAllies(attack.Source);
        FightManager.GetInstance().AddAction(new Heal(allies[Random.Range(0, allies.Count)], attack.CalculatedDamage * healMultiplier));
    }

    public override string GetDescription()
    {
        throw new System.NotImplementedException();
    }
}
