using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalChanceMonsterBase : MonsterBase
{
    [SerializeField] protected float criticalChance;
    [SerializeField] protected float criticalMultiplier;

    // TODO: Make Monster get attack from MonsterBase, Room, etc.
}
