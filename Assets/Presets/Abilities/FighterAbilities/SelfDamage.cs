using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SelfDamage", menuName = "Abilities/Fighter/Self Damage")]
public class SelfDamage : TriggeredFighterAbility
{
    [SerializeField] private float damage;
    [SerializeField] private float damageChange;
    [SerializeField] private int count;
    

    protected override void Activate(Fighter self)
    {
        FightManager.GetInstance().AddAction(new TakeDamage(new Damage(null, self, damage)));
        damage += damageChange;
        if (count > 0) count--;
        if (count == 0) FightManager.GetInstance().AddAction(new RemoveAbility(self, this));
    }

    public override string GetDescription()
    {
        return string.Format(description, damage, damageChange, count);
    }
}
