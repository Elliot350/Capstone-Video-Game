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
    [SerializeField] protected Slider slider;
    [SerializeField] protected Image image, alertImage;
    [SerializeField] protected Room room;
    public float healthMultiplier = 1f, damageMultiplier = 1f;

    public virtual void TakeDamage(float amount) 
    {
        health -= amount;
        slider.value = health;
        if (health <= 0)
            Die();
    }

    public virtual void Heal(float amount) 
    {
        health += amount;
        if (health > maxHealth)
            health = maxHealth;
    }

    public virtual void Attack(List<Monster> fighters) 
    {
        alertImage.gameObject.SetActive(true);
        fighters[0].TakeDamage(Mathf.RoundToInt(damage * damageMultiplier));
    }

    public virtual void Attack(List<Hero> fighters) 
    {
        Debug.Log($"{this} is attacking for {damage * damageMultiplier}");
        fighters[0].TakeDamage(Mathf.RoundToInt(damage * damageMultiplier));
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

    public Room GetRoom() {return room;}
    public string GetName() {return displayName;}
    public float GetHealth() {return health;}
    public float GetMaxHealth() {return maxHealth;}
    public float GetDamage() {return damage;}
    public virtual float GetSpeed() {return 1f;}
}
