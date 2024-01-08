using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Location", menuName = "Other/New Location")]
public class LocationData : ScriptableObject
{
    public string locationName;
    public Color backgroundColor;
    public RoomBase hallways;
    public TileBase empty;
    public List<MonsterBase> monsters;
    public List<MonsterBase> startingMonsters;
    public List<RoomBase> rooms;
    public List<RoomBase> startingRooms;
    public List<HeroBase> heroes;
    // public List<TrapBase> traps;
    public List<PartyLayout> partyLayouts;
}
