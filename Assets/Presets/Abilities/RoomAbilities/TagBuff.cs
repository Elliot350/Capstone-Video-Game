using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Kin", menuName = "Abilities/Room/Kin")]
public class TagBuff : RoomAbility
{
    [SerializeField] private List<Tag> tags;
    [SerializeField] private float damageBoost;
    [SerializeField] private float healthBoost;

    [SerializeField] private TargetType targetType;

    public override void CalculateDamage(Room r, List<Fighter> monsters, List<Fighter> heroes)
    {
        if (targetType == TargetType.MONSTER || targetType == TargetType.BOTH)
        {
            foreach (Fighter f in monsters)
            {
                foreach (Tag t in tags)
                {
                    if (f.HasTag(t))
                    {
                        f.IncreaseDamageModifier(damageBoost);
                        f.IncreaseMaxHealthModifier(healthBoost);
                        continue;
                    }
                }
            }
        }

        if (targetType == TargetType.HERO || targetType == TargetType.BOTH)
        {
            foreach (Fighter f in heroes)
            {
                foreach (Tag t in tags)
                {
                    if (f.HasTag(t))
                    {
                        f.IncreaseDamageModifier(damageBoost);
                        f.IncreaseMaxHealthModifier(healthBoost);
                        continue;
                    }
                }
            }
        }
    }

    public override string GetDescription()
    {
        return string.Format(description, Tag.FormatTags(tags), damageBoost, healthBoost);
    }
}
