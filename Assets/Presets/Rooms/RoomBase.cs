using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Room Preset", menuName = "Presets/Room Preset")]
public class RoomBase : ScriptableObject
{
    [SerializeField] protected string displayName;
    [SerializeField] protected int cost;
    [SerializeField] protected string description;
    [SerializeField] protected int monsterCapacity;
    [SerializeField] protected int trapCapacity;
    [SerializeField] protected Sprite sprite;
    [SerializeField] protected RuleTile tile;
    

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

    public virtual void RoomDefeated(Room room) {}
    public int GetCost() {return cost;}
    public int GetMonster() {return monsterCapacity;}
    public int GetTrap() {return trapCapacity;}
    public Sprite GetSprite() {return sprite;}
    public RuleTile GetTile() {return tile;}
    public string GetName() {return displayName;}
    public string GetDescription() {return string.Format(description, GetMonster());}
}
