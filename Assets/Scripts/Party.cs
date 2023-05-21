using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{
    // TODO: This could be actually Hero, with them all children of the fight viewer and they stay there.
    public List<Hero> heroes = new List<Hero>();

    private void Start()
    {
        transform.position = DungeonManager.GetInstance().GetEntrance();
        PartyManager.GetInstance().GenerateCompletePath();
    }

    public void AddHero(Hero hero) {
        heroes.Add(hero);
    }

    // public void AddHero(HeroBase hero)
    // {
    //     heroes.Add(hero);
    // }

    public void AddHero(List<Hero> heroesList) {
        foreach (Hero h in heroesList)
        {
            heroes.Add(h);
        }
    }

    public void StartFight() {
        Debug.LogWarning($"StartFight() has not yet been implemented");
    }

    public void ListHeroes() {
        foreach (Hero hero in heroes)
        {
            Debug.Log(hero.GetName());
        }
    }

    public void HeroDead(Hero hero)
    {
        heroes.Remove(hero);
        // Debug.Log($"Hero died! Can't do anything about it");
    }

    // TODO: Re-implement traps
    public void DamageHero(int damageAmount)
    {
        // heroes[Random.Range(0, heroes.Count - 1)].TakeDamage(damageAmount);
    }
}
