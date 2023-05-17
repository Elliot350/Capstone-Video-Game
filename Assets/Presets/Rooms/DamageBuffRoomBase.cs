using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageBuffRoom", menuName = "Presets/Rooms/Damage Buff Room Base")]
public class DamageBuffRoomBase : RoomBase
{
    [SerializeField] protected float damageBoost;
    [SerializeField] protected List<Tag> tags;

    public override float CalculateDamageMultiplier(Monster monster)
    {
        foreach (Tag t in tags)
        {
            if (monster.monsterBase.GetTags().Contains(t))
                return base.CalculateDamageMultiplier(monster) + damageBoost;
        }
        return base.CalculateDamageMultiplier(monster);
    }

    public override string GetDescription()
    {
        return string.Format(description, damageBoost * 100, Tag.FormatTags(tags));
    }
}
