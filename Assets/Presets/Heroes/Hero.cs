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
        animator.SetBool("Monster", false);
    }

    public override void Attack(List<Monster> fighters)
    {
        float attackDamage = fighterBase.GetDamage() * CalculateDamageMultiplier();
        Fighter target = fighterBase.DecideTarget(fighters);
        base.Attack(fighters);
        target.TakeDamage(new Damage(this, target, attackDamage));
    }

    public override void TakeDamage(Damage attack)
    {
        base.TakeDamage(attack);
        PartyManager.GetInstance().HeroHurt(this);
    }

    public override void Die(Damage attack)
    {
        PartyManager.GetInstance().HeroDied(this);
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
