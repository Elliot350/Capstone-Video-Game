using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Fighter
{
    public MonsterBase monsterBase;
    // TODO: Make this in Fighter instead
    [SerializeField] private Animator animator;

    public void SetType(MonsterBase monsterBase, Room room)
    {
        this.monsterBase = monsterBase;
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
        monsterBase.OnDeath(this);
        FightManager.GetInstance().FighterDied(this);
        Destroy(gameObject);
    }

    public override void Attack(List<Hero> fighters)
    {
        // Debug.Log($"{this} is attackig for {damage * damageMultiplier}");
        float attackDamage = damage * CalculateDamageMultiplier();
        Fighter target = monsterBase.DecideTarget(fighters);
        Debug.Log($"Attacking for {attackDamage}");
        target.TakeDamage(attackDamage);
        animator.SetTrigger("Attack");
        monsterBase.OnAttack();
    }

    private float CalculateDamageMultiplier()
    {
        return 1f + room.roomBase.CalculateDamageMultiplier(this) + monsterBase.GetDamageMultiplier();
    }

    public override float GetSpeed()
    {
        return monsterBase.GetSpeed();
    }

    public Sprite GetSprite()
    {
        return image.sprite;
    }
}
