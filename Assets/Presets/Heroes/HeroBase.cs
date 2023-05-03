using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero Preset", menuName = "Presets/Hero Preset")]
public class HeroBase : FighterBase
{
    public void SetType(Hero hero)
    {
        hero.displayName = displayName;
        hero.maxHealth = maxHealth;
        hero.health = maxHealth;
        hero.damage = damage;
    }
}
