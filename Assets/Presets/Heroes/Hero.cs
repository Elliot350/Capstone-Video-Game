using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : Fighter
{
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetType(HeroPreset heroPreset)
    {
        displayName = heroPreset.displayName;
        maxHealth = heroPreset.health;
        health = maxHealth;
        damage = heroPreset.damage;
        spriteRenderer.sprite = heroPreset.sprite;
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
        health -= amount;
        Debug.Log("Ouch! I just took some damage!");
        slider.value = health;
        if (health <= 0)
            Die();
    }
}
