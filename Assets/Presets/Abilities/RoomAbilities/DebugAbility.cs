using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Debug", menuName = "Abilities/Debug/Room")]
public class DebugAbility : RoomAbility
{
    public override void BattleEnd(Room r, List<Fighter> monsters, List<Fighter> heroes)
    {
        Debug.Log($"BattleEnd");
    }

    public override void BattleStart(Room r, List<Fighter> monsters, List<Fighter> heroes)
    {
        Debug.Log($"BattleStart");
    }

    public override void CalculateDamage(Room r, List<Fighter> monsters, List<Fighter> heroes)
    {
        // Debug.Log($"CalculateDamage");
    }

    public override void CalculateMaxHealth(Room r, List<Fighter> monsters, List<Fighter> heroes)
    {
        // Debug.Log($"CalculateMaxHealth");
    }

    public override bool CanAddMonster(Room r, MonsterBase monster)
    {
        Debug.Log($"CanAddMonster");
        return true;
    }

    public override bool CanAddTrap(Room r, TrapBase trap)
    {
        Debug.Log($"CanAddTrap");
        return true;
    }

    public override void OnFighterSummoned(Room r, Fighter f)
    {
        Debug.Log($"FighterSummoned");
    }

    public override string GetDescription()
    {
        return string.Format(description);
    }

    public override void OnFighterDied(Room r, Fighter f)
    {
        Debug.Log($"OnFighterDied");
    }

    public override void PartyWon(Party p)
    {
        Debug.Log($"PartyWon");
    }

    public override void RoomBuilt(Room r)
    {
        Debug.Log($"RoomBuilt");
    }
}
