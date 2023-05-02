using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public TrapPreset trap;

    private string displayName;
    private int damage;
    private float triggerChance;
    [SerializeField] SpriteRenderer spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Trap created");
        spriteRenderer.gameObject.SetActive(false);
        
    }

    public void SetType(TrapPreset trapPreset)
    {
        trap = trapPreset;
        displayName = trap.displayName;
        damage = trap.damage;
        triggerChance = trap.triggerChance;
        spriteRenderer.sprite = trap.sprite;
    }

    public void PartyEntered(Party party)
    {
        trap.PartyEntered(party);
    }
}
