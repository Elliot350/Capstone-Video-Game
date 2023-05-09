using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Fighter
{
    public Slider slider;
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
    }

    public override void Die()
    {
        transform.parent.GetComponent<Party>().HeroDead(this);
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        slider.value = health;
    }

    public override float GetSpeed()
    {
        return heroBase.GetSpeed();
    }
}
