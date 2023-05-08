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

    public virtual void AddRoom(Room room)
    {
        // Debug.Log($"Adding to rooms");
        DungeonManager.GetInstance().rooms.Add(room);
        room.Highlight(false);
    }

    public virtual void RoomDefeated(Room room) {}
    public virtual void RoomBuilt(Room room) {}
    public virtual void MonsterAdded(Room room, Monster monster) {}
    public virtual void TrapAdded(Room room, Trap trap) {}
    public virtual float CalculateDamage(Monster monster) {return 1f;}
    public virtual float CalculateHealth(Monster monster) {return 1f;}

    public int GetCost() {return cost;}
    public int GetMonster() {return monsterCapacity;}
    public int GetTrap() {return trapCapacity;}
    public Sprite GetSprite() {return sprite;}
    public RuleTile GetTile() {return tile;}
    public string GetName() {return displayName;}
    public virtual string GetDescription() {return string.Format(description, GetMonster());}
}
