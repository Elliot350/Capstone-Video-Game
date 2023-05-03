using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Room Preset", menuName = "Presets/Rooms/Boss Room Preset")]
public class BossRoom : RoomBase
{
    public override void AddRoom(Room room)
    {
        DungeonManager.GetInstance().bossRoom = Vector3Int.FloorToInt(room.transform.position);
        room.Highlight(false);
    }
}
