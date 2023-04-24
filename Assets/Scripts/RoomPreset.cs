using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Room Preset", menuName = "Presets/Room Preset")]
public class RoomPreset : ScriptableObject
{
    public string displayName;
    public int cost;
    // public GameObject prefab;
    public int monsterCapacity;
    public int trapCapacity;
    public RuleTile tile;
}
