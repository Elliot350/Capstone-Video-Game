using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Room Preset", menuName = "Presets/Rooms/Boss Room Preset")]
public class BossRoom : RoomBase
{
    public override void AddRoom(Room room)
    {
        DungeonManager.GetInstance().BossRoomPlaced(Vector3Int.FloorToInt(room.transform.position));
        room.Highlight(false);
        UIManager.GetInstance().OpenBossMenu();
    }

    public override void RoomDefeated(Room room)
    {
        PartyManager.GetInstance().CompletedDungeon();
    }
}
