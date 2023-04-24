using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Preset", menuName = "Presets/Monster Preset")]
public class MonsterPreset : ScriptableObject
{
    public string displayName;
    public int cost;
    public int health;
    public int damage;
    public int gold;
    public Sprite sprite;
    public Monster prefab;
}
