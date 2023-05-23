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

    public override void Attack(List<Monster> fighters)
    {
        float attackDamage = fighterBase.GetDamage() * CalculateDamageMultiplier();
        Fighter target = fighterBase.DecideTarget(fighters);
        base.Attack(fighters);
        target.TakeDamage(new Damage(this, target, attackDamage));
    }

    public override void Die(Damage attack)
    {
        PartyManager.GetInstance().GetParty().HeroDead(this);
        base.Die(attack);
    }

    public override Sprite GetSprite()
    {
        return fighterBase.GetSprite();
    }

    public override float GetSpeed()
    {
        return fighterBase.GetSpeed();
    }
}
