using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Fighter
{
    public Slider slider;
    public HeroBase hero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetType(HeroBase heroBase)
    {
        hero = heroBase;
        displayName = hero.GetName();
        maxHealth = hero.GetMaxHealth();
        health = maxHealth;
        damage = hero.GetDamage();
        slider.minValue = 0;
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public override void Die()
    {
        transform.parent.GetComponent<Party>().HeroDead(this);
    }

    public override void TakeDamage(int amount)
    {
        slider.value = health;
        base.TakeDamage(amount);
    }
}
