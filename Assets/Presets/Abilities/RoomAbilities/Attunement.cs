using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attunement", menuName = "Abilities/Rooms/Attunement")]
public class Attunement : RoomAbility
{
    public List<Tag> tags;
    public float damageMultiplier;

    public override float GetDamageMultiplier(Fighter f)
    {
        foreach (Tag t in tags)
        {
            if (f.HasTag(t))
                return damageMultiplier;
        }
        return 0f;
    }

    public override string GetDescription()
    {
        return string.Format(description, damageMultiplier, Tag.FormatTags(tags));
    }
}