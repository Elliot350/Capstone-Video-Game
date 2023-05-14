using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trap : MonoBehaviour
{
    public TrapBase trapBase;

    public string displayName;
    public bool triggered;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Trap created");
        
    }

    private void Update()
    {
        
    }

    public void SetType(TrapBase trapPreset)
    {
        trapBase = trapPreset;
        this.displayName = trapBase.GetName();
    }

    public void PartyEntered(Party party)
    {
        trapBase.PartyEntered(party, this);
    }
}
