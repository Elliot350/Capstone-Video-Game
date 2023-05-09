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


    public virtual string GetDescription() {return string.Format(description, Tag.FormatTags(tags));}
    public List<Tag> GetTags() {return tags;}
    public int GetCost() {return cost;}
}
