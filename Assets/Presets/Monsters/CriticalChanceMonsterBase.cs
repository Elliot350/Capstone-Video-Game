using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Presets/Monsters/Critical Chance Monster")]
public class CriticalChanceMonsterBase : MonsterBase
{
    [SerializeField] protected float criticalChance;
    [SerializeField] protected float criticalMultiplier;

    // TODO: Make Monster get attack from MonsterBase, Room, etc.
    public override float GetDamageMultiplier()
    {
        return Random.Range(0f, 1f) < criticalChance ? criticalMultiplier : 0;
    }

    public override string GetDescription()
    {
        return string.Format(description, Tag.FormatTags(tags), criticalChance * 100, criticalMultiplier * 100);
    }
}
