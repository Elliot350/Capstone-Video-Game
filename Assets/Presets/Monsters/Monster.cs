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

    public override void Attack(List<Hero> fighters)
    {
        // Debug.Log($"{this} is attackig for {damage * damageMultiplier}");
        float attackDamage = damage * CalculateDamageMultiplier();
        Fighter target = fighterBase.DecideTarget(fighters);
        Debug.Log($"Attacking for {attackDamage}");
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
