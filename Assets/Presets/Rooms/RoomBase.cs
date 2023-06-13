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
    [SerializeField] protected List<RoomAbility> abilities;
    [SerializeField] protected Sprite sprite;
    [SerializeField] protected RuleTile tile;

    public virtual void AddRoom(Room room)
    {
        // Debug.Log($"Adding to rooms");
        DungeonManager.GetInstance().GetRooms().Add(room);
        room.Highlight(false);
    }

    public virtual void RoomDefeated(Room room) {}
    public virtual void RoomBuilt(Room room) {}
    public virtual void MonsterAdded(Room room, MonsterBase monster) {}
    public virtual void TrapAdded(Room room, Trap trap) {}
    public virtual void OnMonsterDied(Monster monster) {}
    public virtual float CalculateDamageMultiplier(Fighter f) {return 0f;}
    public virtual float CalculateHealthMultiplier(Monster monster) {return 0f;}

    public int GetCost() {return cost;}
    public int GetMonster() {return monsterCapacity;}
    public int GetTrap() {return trapCapacity;}
    public List<RoomAbility> GetAbilities() {return abilities;}
    public Sprite GetSprite() {return sprite;}
    public RuleTile GetTile() {return tile;}
    public string GetName() {return displayName;}
    public virtual string GetDescription() {return Ability.GetDescriptionFromList(abilities);}
}
