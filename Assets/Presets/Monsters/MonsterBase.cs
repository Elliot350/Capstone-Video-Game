using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Preset", menuName = "Presets/Monster Preset")]
public class MonsterBase : FighterBase
{
    [SerializeField] protected int cost;
    [SerializeField] protected string description;
    [SerializeField] protected List<Tag> tags;

    public virtual void MonsterSpawned() {}
    public override void OnDeath(Fighter fighter) 
    {
        if (fighter.transform.parent.TryGetComponent<Room>(out Room room) && fighter.gameObject.TryGetComponent<Monster>(out Monster monster))
            room.MonsterDied(monster);
    }
    public int GetCost() {return cost;}
    public virtual string GetDescription() 
    {
        return string.Format(description, FormatTags());
    }
    public List<Tag> GetTags() {return tags;}
    public string FormatTags() 
    {
        string tagsString = tags[0].FormatTag();
        for (int i = 1; i < tags.Count; i++)
        {
            tagsString += ", " + tags[i].FormatTag();
        }
        return tagsString;
    }
}
