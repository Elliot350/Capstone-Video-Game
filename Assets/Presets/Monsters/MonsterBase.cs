using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Preset", menuName = "Presets/Monster Preset")]
public class MonsterBase : FighterBase
{
    [SerializeField] protected int cost;
    [SerializeField] protected string description;

    public virtual void MonsterSpawned() {}
    public override void Die(Fighter fighter) 
    {
        if (fighter.transform.parent.TryGetComponent<Room>(out Room room) && fighter.gameObject.TryGetComponent<Monster>(out Monster monster))
            room.MonsterDied(monster);
    }
    public int GetCost() {return cost;}
    public string GetDescription() {return description;}
}
