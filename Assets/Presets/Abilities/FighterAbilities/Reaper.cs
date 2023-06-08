using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Reaper", menuName = "Abilities/Fighter/Reaper")]
public class Reaper : FighterAbility
{
    [SerializeField] private float healthGain;
    [SerializeField] private float damageGain;

    protected override void OnFighterDied(Fighter f, Fighter dead)
    {
        if (damageGain > 0)
            // Maybe this should be an Action, just for consistancy
            f.AddDamage(damageGain);
        if (healthGain > 0)
            FightManager.GetInstance().AddAction(new Heal(f, healthGain));
    }

    public override string GetDescription()
    {
        return string.Format(description, healthGain, damageGain);
    }
}
