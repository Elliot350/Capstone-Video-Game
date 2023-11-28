using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consume", menuName = "Abilities/Fighter/Consume")]
public class Consume : TriggeredFighterAbility
{
    protected override void Activate(Fighter self)
    {
        List<Fighter> allies = FightManager.GetInstance().GetAllies(self);
        if (allies.Count == 0) return;
        Fighter target = allies[Random.Range(0, allies.Count)];
        float healthValue = target.GetHealth();
        float damageValue = target.GetDamage();

        FightManager.GetInstance().AddAction(new Die(target, new Damage(self, target, 0f)));
        FightManager.GetInstance().AddAction(new BuffFighter(self, healthValue, damageValue));
    }

    public override string GetDescription()
    {
        return string.Format(description);
    }
}
