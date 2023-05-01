using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{

    public List<Hero> heroes = new List<Hero>();

    private void Start()
    {
        transform.position = DungeonManager.GetInstance().entrance;
        PartyManager.GetInstance().GenerateCompletePath();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddHero(Hero hero) {
        heroes.Add(hero);
    }

    public void AddHero(List<Hero> heroesList) {
        heroes.AddRange(heroesList);
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
    }

    public void DamageHero(int damageAmount)
    {
        heroes[Random.Range(0, heroes.Count - 1)].TakeDamage(damageAmount);
    }
}
