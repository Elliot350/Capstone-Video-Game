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

    public override void TakeDamage(Fighter source, float amount)
    {
        fighterBase.OnTakenDamage(source, amount);
        base.TakeDamage(source, amount);
    }

    public override void Attack(List<Monster> fighters)
    {
        float attackDamage = fighterBase.GetDamage() * CalculateDamageMultiplier();
        Fighter target = fighterBase.DecideTarget(fighters);
        base.Attack(fighters);
        target.TakeDamage(this, attackDamage);
    }

    public override void Die()
    {
        fighterBase.OnDeath(lastAttacker);
        PartyManager.GetInstance().GetParty().HeroDead(this);
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
