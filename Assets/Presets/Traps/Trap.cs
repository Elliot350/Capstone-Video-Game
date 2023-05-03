using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trap : MonoBehaviour
{
    public TrapBase trap;

    public string displayName;
    public int damage;
    public float triggerChance;
    public SpriteRenderer spriteRenderer;
    public Image image;
    public float countdown;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Trap created");
        spriteRenderer.gameObject.SetActive(false);
        image.gameObject.SetActive(false);
        
    }

    private void Update()
    {
        if (countdown > 0)
        {
            countdown -= Time.deltaTime;
            if (countdown < 0)
            {
                image.gameObject.SetActive(false);
            }
        }
    }

    public void SetType(TrapBase trapPreset)
    {
        trap = trapPreset;
        trap.SetType(this);
    }

    public void PartyEntered(Party party)
    {
        trap.PartyEntered(party, this);
    }
}
