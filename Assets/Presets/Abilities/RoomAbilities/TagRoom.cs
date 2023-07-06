using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TagRoom", menuName = "Abilities/Room/Tag Room")]
public class TagRoom : RoomAbility
{
    [SerializeField] private Tag tag;

    public override void FightStarted(List<Fighter> monsters, List<Fighter> heroes)
    {
        foreach (Fighter f in monsters)
            f.AddTag(tag);
    }

    public override string GetDescription()
    {
        return string.Format(description, tag.Format());
    }
}
