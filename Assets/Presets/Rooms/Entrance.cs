using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entrance Preset", menuName = "Presets/Rooms/Entrance Preset")]
public class Entrance : RoomBase
{
    public override void AddRoom(Room room)
    {
        DungeonManager.GetInstance().EntrancePlaced(Vector3Int.FloorToInt(room.transform.position));
        room.Highlight(false);
    }
}
