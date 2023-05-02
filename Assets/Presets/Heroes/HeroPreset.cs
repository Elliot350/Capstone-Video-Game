using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero Preset", menuName = "Presets/Hero Preset")]
public class HeroPreset : ScriptableObject
{
    public string displayName;
    public int health;
    public int damage;
    public Sprite sprite;
}
