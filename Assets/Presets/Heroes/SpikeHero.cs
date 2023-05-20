using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpikedHero", menuName = "Presets/Hero/Spiked Hero Base")]
public class SpikeHero : HeroBase
{
    [SerializeField] protected float returnDamage;

    public override void OnTakenDamage(Fighter source, float amount)
    {
        source.TakeDamage(returnDamage);
    }
}
