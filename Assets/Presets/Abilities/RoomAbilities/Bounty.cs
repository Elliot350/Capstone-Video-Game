using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bounty", menuName = TEST + "/Bounty")]
public class Bounty : RoomAbility
{
    public const string TEST = "Abilities/Room";
    [SerializeField] private int goldAmount;

    public override void OnFighterDied(Room r, Fighter f)
    {
        if (!f.isMonster) GameManager.GetInstance().GainMoney(goldAmount);
    }

    public override string GetDescription()
    {
        return string.Format(description, goldAmount);
    }
}
