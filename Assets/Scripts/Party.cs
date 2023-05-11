using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{

    public List<HeroBase> heroes = new List<HeroBase>();

    private void Start()
    {
        transform.position = DungeonManager.GetInstance().entrance;
        PartyManager.GetInstance().GenerateCompletePath();
    }

    public void AddHero(Hero hero) {
        heroes.Add(hero.heroBase);
    }

    public void AddHero(HeroBase hero)
    {
        heroes.Add(hero);
    }

    public void AddHero(List<Hero> heroesList) {
        foreach (Hero h in heroesList)
            heroes.Add(h.heroBase);
    }

    public void StartFight() {
        Debug.LogWarning($"StartFight() has not yet been implemented");
    }

    public void ListHeroes() {
        foreach (HeroBase hero in heroes)
        {
            Debug.Log(hero.GetName());
        }
    }

    public void HeroDead(HeroBase hero)
    {
        heroes.Remove(hero);
    }

    // TODO: Re-implement traps
    public void DamageHero(int damageAmount)
    {
        // heroes[Random.Range(0, heroes.Count - 1)].TakeDamage(damageAmount);
    }
}
