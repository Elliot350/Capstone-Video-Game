using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Fighter
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log($"I'm a little monster");
        spriteRenderer.gameObject.SetActive(false);
    }

    public void SetType(MonsterPreset monsterPreset)
    {
        displayName = monsterPreset.displayName;
        maxHealth = monsterPreset.health;
        health = maxHealth;
        damage = monsterPreset.damage;
        spriteRenderer.sprite = monsterPreset.sprite;
    }
}
