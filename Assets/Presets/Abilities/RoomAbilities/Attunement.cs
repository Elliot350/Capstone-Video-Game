using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attunement", menuName = "Abilities/Room/Attunement")]
public class Attunement : RoomAbility
{
    public List<Tag> tags;
    public float damageMultiplier;

    public override void CalculateDamage(Fighter f)
    {
        foreach (Tag t in tags)
        {
            if (f.HasTag(t))
            {
                f.IncreaseDamageModifier(damageMultiplier);
                return;
            }
        }
    }

    public override string GetDescription()
    {
        return string.Format(description, damageMultiplier * 100, Tag.FormatTags(tags));
    }
}
