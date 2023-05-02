using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spike Trap", menuName = "Presets/Traps/Spike Trap")]
public class SpikeTrap : TrapPreset
{
    public override void PartyEntered(Party party) {
        Trigger(party);
    } 
    
    protected override void Trigger(Party party)
    {
        if (Random.Range(0, 1) < triggerChance)
            party.DamageHero(damage);
    }
}
