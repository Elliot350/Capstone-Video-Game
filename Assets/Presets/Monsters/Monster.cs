using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Fighter
{
    public MonsterBase monsterBase;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.gameObject.SetActive(false);
    }

    public void SetType(MonsterBase monsterPreset)
    {
        this.monsterBase = monsterPreset;
        this.monsterBase.SetType(this);
        spriteRenderer.sprite = monsterBase.sprite;
    }

    public override void Die()
    {
        monsterBase.Die(this);
    }
}
