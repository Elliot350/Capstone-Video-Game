using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Fighter
{
    public override void Die(Damage attack)
    {
        room.MonsterDied(this);
        base.Die(attack);
    }

    protected override void SetAnimator()
    {
        isMonster = true;
    }
}
