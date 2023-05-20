using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Fighter
{
    public void SetType(MonsterBase monsterBase, Room room)
    {
        this.fighterBase = monsterBase;
        this.room = room;
        displayName = monsterBase.GetName();
        maxHealth = monsterBase.GetMaxHealth();
        health = maxHealth;
        slider.minValue = 0;
        slider.maxValue = maxHealth;
        slider.value = health;
        damage = monsterBase.GetDamage();
        damageMultiplier = CalculateDamageMultiplier();
        healthMultiplier = 1f + room.roomBase.CalculateHealthMultiplier(this);
        image.sprite = monsterBase.GetSprite();
        alertImage.gameObject.SetActive(false);
    }

    public override void Die()
    {
        Debug.Log($"Monster died!");
        room.MonsterDied(this);
        fighterBase.OnDeath(this);
        FightManager.GetInstance().FighterDied(this);
        animator.SetTrigger("Dead");
    }

    public override void Attack(List<Hero> fighters)
    {
        // Debug.Log($"{this} is attackig for {damage * damageMultiplier}");
        float attackDamage = damage * CalculateDamageMultiplier();
        Fighter target = fighterBase.DecideTarget(fighters);
        Debug.Log($"Attacking for {attackDamage}");
        target.TakeDamage(this, attackDamage);
        base.Attack(fighters);
        fighterBase.OnAttack();
    }

    protected override float CalculateDamageMultiplier()
    {
        return 1f + room.roomBase.CalculateDamageMultiplier(this) + fighterBase.GetDamageMultiplier(this);
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
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
