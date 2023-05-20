using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeathDamageHero", menuName = "Presets/Hero/Death Damage Hero Base")]
public class DeathDamageHero : HeroBase
{
    [SerializeField] protected float deathDamage;

    public override void OnDeath(Fighter source)
    {
        source.TakeDamage(deathDamage);   
    }
}
