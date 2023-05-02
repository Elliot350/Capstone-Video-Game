using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Fighter
{
    public MonsterPreset monster;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.gameObject.SetActive(false);
    }

    public void SetType(MonsterPreset monsterPreset)
    {
        monster = monsterPreset;
        monster.SetType(this);
    }

    public override void Die()
    {
        monster.Die();
    }
}
