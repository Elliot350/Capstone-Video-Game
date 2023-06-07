using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ultimate", menuName = "Abilities/Fighter/Ultimate")]
public class Ultimate : FighterAbility
{
    public override bool CanAddMonster(MonsterBase m, Room r)
    {
        return !r.monsters.Contains(m);
    }

    public override string GetDescription()
    {
        return string.Format(description);
    }
}
