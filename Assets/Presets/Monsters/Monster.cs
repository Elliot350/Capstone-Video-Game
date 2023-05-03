using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Fighter
{
    public MonsterBase monsterPreset;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.gameObject.SetActive(false);
    }

    public void SetType(MonsterBase monsterPreset)
    {
        this.monsterPreset = monsterPreset;
        this.monsterPreset.SetType(this);
    }

    public override void Die()
    {
        monsterPreset.Die(this);
    }
}
