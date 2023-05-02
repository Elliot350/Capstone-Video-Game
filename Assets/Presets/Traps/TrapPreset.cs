using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trap Preset", menuName = "Presets/Trap Preset")]
public class TrapPreset : ScriptableObject
{
    public string displayName;
    public int cost;
    public int damage;
    public float triggerChance;
    public Sprite sprite;

    // Nothing will ever call trigger directly, they will call PartyEntered or others, which the Traps override to call Trigger
    protected virtual void Trigger(Party party) {}
    public virtual void PartyEntered(Party party) {}
    public virtual void PartyExited(Party party) {}
}
