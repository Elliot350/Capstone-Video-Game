using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "Bases/New Monster")]
public class MonsterBase : FighterBase
{
    [SerializeField] protected bool needsAll;
    [SerializeField] protected List<MonsterBase> requirements;

    public virtual void MonsterSpawned() {}

    public bool IsUnlockable() 
    {
        if (needsAll)
            return NeedsAll();
        return NeedsOne();
    }

    private bool NeedsAll()
    {
        foreach (MonsterBase mb in requirements)
        {
            if (!UnlockManager.GetInstance().IsMonsterUnlocked(mb))
                return false;
        }
        return true;
    }

    private bool NeedsOne()
    {
        foreach (MonsterBase mb in requirements)
        {
            if (UnlockManager.GetInstance().IsMonsterUnlocked(mb))
                return true;
        }
        return false;
    }

    public List<MonsterBase> GetRequirements() {return requirements;}
}
