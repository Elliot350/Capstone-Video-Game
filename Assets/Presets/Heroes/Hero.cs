using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Fighter
{
    public void EnterRoom(Room room)
    {
        this.room = room;
    }

    protected override void SetAnimator()
    {
        isMonster = false;
    }
    
    // public override void TakeDamage(Damage attack)
    // {
    //     PartyManager.GetInstance().HeroHurt(this);
    //     base.TakeDamage(attack);
    // }

    // public override void Die(Damage attack)
    // {
    //     PartyManager.GetInstance().HeroDied(this);
    //     base.Die(attack);
    // }
}
