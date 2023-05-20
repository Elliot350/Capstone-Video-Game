using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fighter : MonoBehaviour
{
    [SerializeField] protected string displayName;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;
    [SerializeField] protected float damage;
    [SerializeField] protected List<Tag> tags;
    [SerializeField] protected Slider slider;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Image image, alertImage;

    protected float healthMultiplier = 1f;
    protected float damageMultiplier = 1f;
    protected Fighter lastAttacker;
    protected Room room;
    protected FighterBase fighterBase;

    public void SetType(FighterBase fighterBase)
    {
        this.fighterBase = fighterBase;
        displayName = fighterBase.GetName();
        maxHealth = fighterBase.GetMaxHealth();
        health = maxHealth;
        damage = fighterBase.GetDamage();
        tags = fighterBase.GetTags();
        slider.minValue = 0f;
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
        image.sprite = fighterBase.GetSprite();
        alertImage.gameObject.SetActive(false);
    }

    public void SetType(FighterBase fighterBase, Room room)
    {
        this.room = room;
        SetType(fighterBase);
    }

    public virtual void TakeDamage(Fighter source, float amount) 
    {
        lastAttacker = source;
        TakeDamage(amount);
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        slider.value = health;
        fighterBase.OnTakenDamage(lastAttacker, amount);
        if (health <= 0)
            Die();
    }

    public virtual void Heal(float amount) 
    {
        health += amount;
        fighterBase.OnHeal();
        if (health > maxHealth)
            health = maxHealth;
    }

    public virtual void Attack(List<Monster> fighters) 
    {
        animator.SetTrigger("Attack");
    }

    public virtual void Attack(List<Hero> fighters) 
    {
        animator.SetTrigger("Attack");
    }

    public virtual void DoneAttack()
    {
        alertImage.gameObject.SetActive(false);
    }

    public virtual void Die() 
    {
        Debug.LogWarning($"Not yet implemented Die");
    }

    public void FinishBattle() 
    {
        Debug.LogWarning($"Not yet implemented FinishBattle");
    }

    protected virtual float CalculateDamageMultiplier()
    {
        return 1f + room.roomBase.CalculateDamageMultiplier(this) + fighterBase.GetDamageMultiplier(this);
    }

    public virtual Sprite GetSprite() {return null;}
    public Room GetRoom() {return room;}
    public string GetName() {return displayName;}
    public float GetHealth() {return health;}
    public float GetMaxHealth() {return maxHealth;}
    public float GetDamage() {return damage;}
    public List<Tag> GetTags() {return tags;}
    public virtual float GetSpeed() {return 1f;}
}
