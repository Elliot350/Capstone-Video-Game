using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Preset", menuName = "Presets/Monster Preset")]
public class MonsterBase : FighterBase
{
    [SerializeField] protected int cost;
    [SerializeField] protected string description;
    [SerializeField] protected bool needsAll;
    [SerializeField] protected List<MonsterBase> requirements;
    [SerializeField] protected List<Tag> tags;

    public virtual void MonsterSpawned() {}

    public override void OnDeath(Fighter fighter) 
    {
        if (fighter.transform.parent.TryGetComponent<Room>(out Room room) && fighter.gameObject.TryGetComponent<Monster>(out Monster monster))
            room.MonsterDied(monster);
    }

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

    public virtual string GetDescription() {return string.Format(description, Tag.FormatTags(tags));}
    public List<Tag> GetTags() {return tags;}
    public int GetCost() {return cost;}
}
