using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeathDamageHero", menuName = "Presets/Hero/Death Damage Hero Base")]
public class DeathDamageHero : HeroBase
{
    [SerializeField] protected float deathDamage;

    public override void OnDeath(Damage attack)
    {
        FightManager.GetInstance().AddAction(new TakeDamage(new Damage(attack.target, attack.source, deathDamage)));
        // yield return attack.source.StartCoroutine(attack.source.DoActions());
    }
}
