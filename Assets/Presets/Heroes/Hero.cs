using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Fighter
{
    public HeroBase heroBase;

    public void SetType(HeroBase heroBase)
    {
        this.heroBase = heroBase;
        displayName = this.heroBase.GetName();
        maxHealth = this.heroBase.GetMaxHealth();
        health = maxHealth;
        damage = this.heroBase.GetDamage();
        slider.minValue = 0;
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        image.sprite = heroBase.GetSprite();
        alertImage.gameObject.SetActive(false);
    }

    public void SetType(HeroBase heroBase, Room room)
    {
        SetType(heroBase);
        this.room = room;
    }

    public override void Attack(List<Monster> fighters)
    {
        float attackDamage = heroBase.GetDamage() * CalculateDamageMultiplier();
        Fighter target = heroBase.DecideTarget(fighters);
        base.Attack(fighters);
        target.TakeDamage(attackDamage);
    }

    private float CalculateDamageMultiplier()
    {
        return 1f + heroBase.GetDamageMultiplier();
    }

    public override void Die()
    {
        PartyManager.GetInstance().GetParty().HeroDead(this);
    }

    public override float GetSpeed()
    {
        return heroBase.GetSpeed();
    }
}
