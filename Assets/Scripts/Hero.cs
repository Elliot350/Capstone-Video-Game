using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Fighter
{
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
    }
}
