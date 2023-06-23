using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeathHeal", menuName = "Abilities/Fighter/Death Heal")]
public class DeathHeal : FighterAbility
{
    public float healAmount;

    public override void OnDeath(Damage attack)
    {
        List<Fighter> list = attack.Target.IsMonster() ? FightManager.GetInstance().GetMonsters() : FightManager.GetInstance().GetHeroes();

        foreach (Fighter f in list)
        {
            if (f != attack.Target)
            {
                FightManager.GetInstance().AddAction(new Heal(f, healAmount));
            }
        }
    }

    public override string GetDescription()
    {
        return string.Format(description, healAmount);
    }
}
