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
        animator.SetBool("Monster", true);
    }

    public override void Attack(List<Hero> fighters)
    {
        float damageMultiplier = CalculateDamageMultiplier();
        float attackDamage = damage * damageMultiplier;
        Fighter target = fighterBase.DecideTarget(fighters);
        Debug.Log($"Attacking for {attackDamage} ({damage} * {damageMultiplier})");
        target.TakeDamage(new Damage(this, target, attackDamage));
        base.Attack(fighters);
        fighterBase.OnAttack();
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
