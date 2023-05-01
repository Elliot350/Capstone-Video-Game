using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
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
        displayName = trapPreset.displayName;
        damage = trapPreset.damage;
        triggerChance = trapPreset.triggerChance;
        spriteRenderer.sprite = trapPreset.sprite;
    }

    public void Trigger()
    {
        if (Random.Range(0, 1) > triggerChance)
        {
            PartyManager.GetInstance().GetParty().DamageHero(damage);
        }
    }
}
