using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageAll", menuName = "Abilities/Fighter/Damage All")]
public class DamageAll : TriggeredFighterAbility
{
    [SerializeField] private float damage;

    public override string GetDescription()
    {
        return string.Format(description, damage);
    }

    protected override void Activate(Fighter target)
    {
        foreach (Fighter f in FightManager.GetInstance().GetFighters())
        {
            if (f != target) FightManager.GetInstance().AddAction(new TakeDamage(new Damage(target, f, damage)));
        }
    }
}
