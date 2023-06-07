using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Fighter
{
    [SerializeField] private int gold;

    // Not sure if this will work
    // public override void SetType(FighterBase fighterBase)
    // {
    //     base.SetType(fighterBase);
    //     if (fighterBase is HeroBase)
    //     {
    //         HeroBase temp = (HeroBase) fighterBase;
    //         gold = temp.GetGold();
    //     }
    // }

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

    public override void Die(Damage attack)
    {
        GameManager.GetInstance().GainMoney(gold);
        PartyManager.GetInstance().HeroDied(this);
        base.Die(attack);
    }
}
