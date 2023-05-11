using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Fighter
{
    public MonsterBase monsterBase;
    // public Sprite sprite;
    public Image image, alertImage;
    public Room room;


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
        damageMultiplier = room.roomBase.CalculateDamage(this);
        healthMultiplier = room.roomBase.CalculateHealth(this);
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
        Debug.Log($"{this} is attackig for {damage * damageMultiplier}");
        monsterBase.DecideTarget(fighters).TakeDamage(damage * damageMultiplier);
        alertImage.gameObject.SetActive(true);
    }

    public override void DoneAttack()
    {
        alertImage.gameObject.SetActive(false);
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
