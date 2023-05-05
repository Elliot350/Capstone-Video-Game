using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trap", menuName = "Presets/Traps/Single Damage Trap")]
public class SingleDamageTrap : TrapBase
{
    public override void PartyEntered(Party party, Trap trap) {
        if (Random.Range(0f, 1f) <= triggerChance)
            Trigger(party, trap);
    }
    
    protected override void Trigger(Party party, Trap trap)
    {
        base.Trigger(party, trap);
        party.DamageHero(damage);
    }
}
