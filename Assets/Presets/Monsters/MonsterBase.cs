using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Bases/New Monster")]
public class MonsterBase : FighterBase
{
    [SerializeField] protected List<Requirement> requirements;

    public List<Requirement> GetRequirements() 
    {
        return requirements;
    }
}
