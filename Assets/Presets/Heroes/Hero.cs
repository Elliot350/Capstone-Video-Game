using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Fighter
{
    [SerializeField] private int gold;

    // Not sure if this will work
    public override void SetBase(FighterBase fighterBase)
    {
        base.SetBase(fighterBase);
        if (fighterBase is HeroBase)
        {
            HeroBase temp = (HeroBase) fighterBase;
            gold = temp.GetGold();
        }
    }

    public void EnterRoom(Room room)
    {
        this.room = room;
    }

    protected override void SetTypes()
    {
        isMonster = false;
        isBoss = false;
    }

    public override void Die(Damage attack)
    {
        GameManager.GetInstance().GainMoney(gold);
        PartyManager.GetInstance().HeroDied(this);
        base.Die(attack);
    }
}
