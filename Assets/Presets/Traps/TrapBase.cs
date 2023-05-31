using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Trap Preset", menuName = "Presets/Trap Base")]
public class TrapBase : ScriptableObject
{
    [SerializeField] protected string displayName;
    [SerializeField] protected int cost;
    [SerializeField] protected string description;
    [SerializeField] protected List<TrapAbility> abilities;
    [SerializeField] protected float triggerChance;
    [SerializeField] protected Sprite sprite;
    [SerializeField] protected float alertDisplayTime = 1f;

    // Nothing will ever call trigger directly, they will call PartyEntered or others, which the Traps override to call Trigger
    protected virtual void Trigger(Party party, Trap trap) {
        trap.gameObject.GetComponent<Room>().TrapTriggered();
        trap.triggered = true;
    }
    public virtual void PartyEntered(Party party, Trap trap) {}
    public virtual void PartyExited(Party party, Trap trap) {}

    public string GetName() {return displayName;}
    public int GetCost() {return cost;}
    public float GetChance() {return triggerChance;}
    public Sprite GetSprite() {return sprite;}
    public List<TrapAbility> GetAbilities() {return abilities;}
    public virtual string GetDescription() 
    {
        if (abilities.Count == 0)
            return "<i>No abilities</i>";
        string text = "";
        foreach (TrapAbility a in abilities)
            text += a.GetAbility();
        return text;
    }
}
