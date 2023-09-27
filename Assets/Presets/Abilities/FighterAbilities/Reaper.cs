using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reaper", menuName = "Abilities/Fighter/Reaper")]
public class Reaper : FighterAbility
{
    [SerializeField] private float healthGain;
    [SerializeField] private float damageGain;

    public override void OnFighterDied(Fighter f, Fighter dead)
    {
        // if (damageGain > 0)
            // Maybe this should be an Action, just for consistancy

        FightManager.GetInstance().AddAction(new BuffMonster(f, healthGain, damageGain));
        // if (healthGain > 0)
            // FightManager.GetInstance().AddAction(new Heal(f, healthGain));
    }

    public override string GetDescription()
    {
        return string.Format(description, healthGain, damageGain);
    }
}
