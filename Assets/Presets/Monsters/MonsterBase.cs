using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Preset", menuName = "Presets/Monster Preset")]
public class MonsterBase : FighterBase
{
    [SerializeField] protected int cost;
    [SerializeField] protected string description;
    [SerializeField] List<Tag> tags;

    public virtual void MonsterSpawned() {}
    public override void OnDeath(Fighter fighter) 
    {
        if (fighter.transform.parent.TryGetComponent<Room>(out Room room) && fighter.gameObject.TryGetComponent<Monster>(out Monster monster))
            room.MonsterDied(monster);
    }
    public int GetCost() {return cost;}
    public virtual string GetDescription() 
    {
        string displayTags = tags[0].FormatTag();
        for (int i = 1; i < tags.Count; i++)
            displayTags += ", " + tags[i].FormatTag();
        return string.Format(description, displayTags);
    }
    public List<Tag> GetTags() {return tags;}
}
