using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleTagRoom", menuName = "Abilities/Room/Single Monster Tag")]
public class SingleTagRoom : RoomAbility
{
    [SerializeField] private Tag singleTag;

    public override bool CanAddMonster(Room r, MonsterBase monster)
    {
        return monster.GetTags().Contains(singleTag);
    }

    public override string GetDescription()
    {
        return string.Format(description, singleTag.FormatTag());
    }
}
