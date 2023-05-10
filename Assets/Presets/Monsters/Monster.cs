using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Fighter
{
    public MonsterBase monsterBase;
    public Sprite sprite;


    public void SetType(MonsterBase monsterBase)
    {
        this.monsterBase = monsterBase;
        displayName = monsterBase.GetName();
        maxHealth = monsterBase.GetMaxHealth();
        health = maxHealth;
        damage = monsterBase.GetDamage();
        damageMultiplier = transform.parent.GetComponent<Room>().roomBase.CalculateDamage(this);
        healthMultiplier = transform.parent.GetComponent<Room>().roomBase.CalculateHealth(this);
        sprite = monsterBase.GetSprite();
    }

    public override void Die()
    {
        Debug.Log($"Monster died!");
        transform.parent.GetComponent<Room>().MonsterDied(this);
        monsterBase.OnDeath(this);
    }

    public override void Attack(List<Hero> fighters)
    {
        Debug.Log($"{this} is attackig for {damage * damageMultiplier}");
        monsterBase.DecideTarget(fighters).TakeDamage(damage * damageMultiplier);
    }

    public override float GetSpeed()
    {
        return monsterBase.GetSpeed();
    }
}
