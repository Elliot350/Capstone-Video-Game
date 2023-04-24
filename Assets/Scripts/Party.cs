using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : MonoBehaviour
{

    public List<Hero> heroes;

    private void Start()
    {
        transform.position = DungeonManager.GetInstance().entrance;
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
}
