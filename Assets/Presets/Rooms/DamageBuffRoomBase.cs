using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageBuffRoom", menuName = "Presets/Rooms/Damage Buff Room Base")]
public class DamageBuffRoomBase : RoomBase
{
    [SerializeField] protected float damageBoost;
    [SerializeField] protected List<Tag> tags;

    public override void MonsterAdded(Room room, Monster monster)
    {
        foreach (Tag t in tags)
        {
            if (monster.monsterBase.GetTags().Contains(t))
            {
                monster.damageMultiplier += damageBoost;
                return;
            }
        }
    }

    public override string GetDescription()
    {
        string displayTags = tags[0].FormatTag();
        for (int i = 1; i < tags.Count; i++)
        {
            displayTags += ", " + tags[i].FormatTag();
        }
        return string.Format(description, damageBoost * 100, displayTags);
    }
}
