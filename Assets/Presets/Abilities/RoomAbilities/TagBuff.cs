using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kin", menuName = "Abilities/Room/Kin")]
public class TagBuff : RoomAbility
{
    [SerializeField] private List<Tag> tags;
    [SerializeField] private float damageBoost;
    [SerializeField] private float healthBoost;

    public override void CalculateDamage(Fighter f)
    {
        foreach (Tag t in tags)
        {
            if (f.HasTag(t))
            {
                f.IncreaseDamageModifier(damageBoost);
                f.IncreaseMaxHealthModifier(healthBoost);
                return;
            }
        }
    }

    public override string GetDescription()
    {
        return string.Format(description, Tag.FormatTags(tags), damageBoost, healthBoost);
    }
}
