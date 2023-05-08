using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageBuffRoom", menuName = "Presets/Rooms/Damage Buff Room Base")]
public class DamageBuffRoomBase : RoomBase
{
    [SerializeField] protected float damageBoost;
    [SerializeField] protected List<Tag> tags;

    public override float CalculateDamage(Monster monster)
    {
        foreach (Tag t in tags)
        {
            if (monster.monsterBase.GetTags().Contains(t))
                return base.CalculateDamage(monster) + damageBoost;
        }
        return base.CalculateDamage(monster);
    }

    public override string GetDescription()
    {
        return string.Format(description, damageBoost * 100, Tag.FormatTags(tags));
    }
}
