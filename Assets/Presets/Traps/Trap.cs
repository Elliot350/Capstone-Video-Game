using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trap : MonoBehaviour
{
    public TrapBase trapBase;

    public string displayName;
    public bool triggered;
    private List<TrapAbility> abilities;
    private Sprite sprite;

    public void SetType(TrapBase trapPreset)
    {
        trapBase = trapPreset;
        this.displayName = trapBase.GetName();
        abilities = new List<TrapAbility>(trapBase.GetAbilities());
    }

    public void PartyEntered(Party party)
    {
        foreach (TrapAbility a in abilities)
            a.PartyEntered(party);
    }
}
