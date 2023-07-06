using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kin", menuName = "Abilities/Fighter/Kin")]
public class Kin : FighterAbility
{
    [SerializeField] private Tag tag;
    [SerializeField] private float damageGain;
    [SerializeField] private float healthGain;

    private int GetCount(Fighter f)
    {
        int count = 0;
        foreach (Fighter fighter in FightManager.GetInstance().GetAllies(f))
            if (fighter.HasTag(tag))
                count++;
        return count;
    }

    public override void CalculateDamage(Fighter f)
    {
        f.IncreaseDamageModifier(GetCount(f) * damageGain);
    }

    public override void CalculateMaxHealth(Fighter f)
    {
        f.IncreaseMaxHealthModifier(GetCount(f) * healthGain);
    }

    public override string GetDescription()
    {
        return string.Format(description, tag.Format(), damageGain, healthGain);
    }
}