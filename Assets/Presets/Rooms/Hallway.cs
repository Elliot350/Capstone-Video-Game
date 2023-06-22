using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hallway", menuName = "Bases/Other/New Hallway")]
public class Hallway : RoomBase
{
    public override void AddRoom(Room room)
    {
        DungeonManager.GetInstance().GetHallways().Add(room);
        room.Highlight(false);
    }
}
