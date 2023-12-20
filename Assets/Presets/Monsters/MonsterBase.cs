using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Bases/New Monster")]
public class MonsterBase : FighterBase
{
    [SerializeField] protected List<Requirement> requirements;

    public bool IsUnlockable() 
    {
        // Check to see if all the requirements are valid
        foreach (Requirement r in requirements)
        {
            if (!r.IsValid())
                return false;
        }
        return true;
    }

    public string GetRequirementsAsString() 
    {
        string text = "";
        foreach (Requirement r in requirements)
        {
            text += r.ToString() + "\n";
        }
        return text;
    }
}
