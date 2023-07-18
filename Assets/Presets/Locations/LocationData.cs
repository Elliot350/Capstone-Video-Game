using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Location", menuName = "Other/New Location")]
public class LocationData : ScriptableObject
{
    public string locationName;
    public RoomBase hallways;
    public RoomBase empty;
    public List<MonsterBase> monsters;
    public List<RoomBase> rooms;
    public List<HeroBase> heroes;
    public List<TrapBase> traps;
    public List<PartyLayout> partyLayouts;
}
