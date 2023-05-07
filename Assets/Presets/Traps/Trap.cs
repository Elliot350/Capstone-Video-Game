using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trap : MonoBehaviour
{
    public TrapBase trapBase;

    public string displayName;
    public SpriteRenderer spriteRenderer;
    public Image image;
    public float countdown;
    public bool triggered;
    
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
        trapBase = trapPreset;
        trapBase.SetType(this);
        displayName = trapBase.GetName();
        spriteRenderer.sprite = trapBase.GetSprite();
    }

    public void PartyEntered(Party party)
    {
        trapBase.PartyEntered(party, this);
    }
}
