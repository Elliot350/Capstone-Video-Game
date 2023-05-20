using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageBuffRoom", menuName = "Presets/Rooms/Damage Buff Room Base")]
public class DamageBuffRoomBase : RoomBase
{
    [SerializeField] protected float damageBoost;
    [SerializeField] protected List<Tag> tags;

    public override float CalculateDamageMultiplier(Fighter f)
    {
        if (f is Hero)
            return 0f;

        foreach (Tag t in tags)
        {
            if (f.GetTags().Contains(t))
                return base.CalculateDamageMultiplier(f) + damageBoost;
        }
        return base.CalculateDamageMultiplier(f);
    }

    public override string GetDescription()
    {
        return string.Format(description, damageBoost * 100, Tag.FormatTags(tags));
    }
}
