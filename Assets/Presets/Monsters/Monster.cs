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

    public void SetType(MonsterBase monsterBase)
    {
        this.monsterBase = monsterBase;
        displayName = monsterBase.GetName();
        maxHealth = monsterBase.GetMaxHealth();
        health = maxHealth;
        damage = monsterBase.GetDamage();
        spriteRenderer.sprite = monsterBase.GetSprite();
    }

    public override void Die()
    {
        Debug.Log($"Monster died!");
        transform.parent.GetComponent<Room>().MonsterDied(this);
        monsterBase.OnDeath(this);
    }

    public override float GetSpeed()
    {
        return monsterBase.GetSpeed();
    }
}
