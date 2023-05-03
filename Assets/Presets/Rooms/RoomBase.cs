using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Room Preset", menuName = "Presets/Room Preset")]
public class RoomBase : ScriptableObject
{
    public string displayName;
    public int cost;
    public int monsterCapacity;
    public int trapCapacity;
    public RuleTile tile;

    // Room calls PartyEntered on each trap
    public virtual void PartyEntered(Room room, Party party) {
        foreach (Trap roomTrap in room.traps)
            roomTrap.trap.PartyEntered(party, roomTrap);
    }

    public virtual void SetType(Room room)
    {
        room.displayName = displayName;
        room.monsterCapacity = monsterCapacity;
        room.trapCapacity = trapCapacity;
        
    }

    public virtual void AddRoom(Room room)
    {
        Debug.Log($"Adding to rooms");
        DungeonManager.GetInstance().rooms.Add(room);
        room.Highlight(false);
    }
}
