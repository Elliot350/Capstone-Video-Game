using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleMonsterRoom", menuName = "Abilities/Room/Single Monster Room")]
public class SingleMonsterRoom : RoomAbility
{
    [SerializeField] private MonsterBase singleMonster;

    public override bool CanAddMonster(Room r, MonsterBase monster)
    {
        return singleMonster == monster;
    }

    public override string GetDescription()
    {
        return string.Format(description, singleMonster.GetName());
    }
}
