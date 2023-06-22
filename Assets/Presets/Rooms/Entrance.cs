using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entrance", menuName = "Bases/Other/New Entrance")]
public class Entrance : RoomBase
{
    public override void AddRoom(Room room)
    {
        DungeonManager.GetInstance().EntrancePlaced(Vector3Int.FloorToInt(room.transform.position));
        room.Highlight(false);
    }
}
