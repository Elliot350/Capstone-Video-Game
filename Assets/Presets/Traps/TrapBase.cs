using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Trap Preset", menuName = "Presets/Trap Preset")]
public class TrapBase : ScriptableObject
{
    [SerializeField] protected string displayName;
    [SerializeField] protected int cost;
    [SerializeField] protected int damage;
    [SerializeField] protected float triggerChance;
    [SerializeField] protected Sprite sprite;

    // Nothing will ever call trigger directly, they will call PartyEntered or others, which the Traps override to call Trigger
    protected virtual void Trigger(Party party) {}
    public virtual void PartyEntered(Party party) {}
    public virtual void PartyExited(Party party) {}
    public virtual void SetType(Trap trap) {
        trap.displayName = displayName;
        trap.spriteRenderer.sprite = sprite;
    }
    public int GetCost() {return cost;}
}
