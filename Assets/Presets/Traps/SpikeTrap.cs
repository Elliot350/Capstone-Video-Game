using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spike Trap", menuName = "Presets/Traps/Spike Trap")]
public class SpikeTrap : TrapBase
{
    public override void PartyEntered(Party party) {
        if (Random.Range(0f, 1f) <= triggerChance)
            Trigger(party);
    } 
    
    protected override void Trigger(Party party)
    {
        party.DamageHero(damage);
    }
}
