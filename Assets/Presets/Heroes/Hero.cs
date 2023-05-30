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

    public override void Attack(List<Monster> fighters)
    {
        float attackDamage = fighterBase.GetDamage() * CalculateDamageMultiplier();
        Fighter target = fighterBase.DecideTarget(fighters);
        Damage attack = new Damage(this, target, attackDamage);
        foreach (Ability a in abilities)
            a.OnAttack(attack);
        CatchUpAbilities();
        animator.SetBool("Monster", isMonster);
        animator.SetTrigger("Attack");
        target.TakeDamage(attack);
    }

    public override void TakeDamage(Damage attack)
    {
        PartyManager.GetInstance().HeroHurt(this);
        base.TakeDamage(attack);
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
