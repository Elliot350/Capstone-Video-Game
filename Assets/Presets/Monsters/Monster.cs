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

    public override void Attack(List<Hero> fighters)
    {
        float damageMultiplier = CalculateDamageMultiplier();
        float attackDamage = damage * damageMultiplier;
        Fighter target = fighterBase.DecideTarget(fighters);
        Damage attack = new Damage(this, target, attackDamage);
        foreach (Ability a in abilities)
            a.OnAttack(attack);
        animator.SetBool("Monster", isMonster);
        animator.SetTrigger("Attack");
        target.TakeDamage(new Damage(this, target, attackDamage));
    }
    
    public override float GetSpeed()
    {
        return fighterBase.GetSpeed();
    }

    public override Sprite GetSprite()
    {
        return image.sprite;
    }
}
