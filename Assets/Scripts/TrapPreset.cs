using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trap Preset", menuName = "Presets/Trap Preset")]
public class TrapPreset : ScriptableObject
{
    public string displayName;
    public int cost;
    public int damage;
    public float triggerChance;
    public Sprite sprite;
    public Trap prefab;
}
