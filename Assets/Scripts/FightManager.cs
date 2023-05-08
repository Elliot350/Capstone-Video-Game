using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    private static FightManager instance;

    public List<Fighter> fighters;

    private void Awake()
    {
        instance = this;
    }

    public static FightManager GetInstance()
    {
        return instance;
    }

    public IEnumerator StartFight(List<Hero> heroes, List<Monster> monsters, Room room)
    {
        if (heroes.Count == 0 || monsters.Count == 0) 
            yield break;

        fighters = new List<Fighter>();
        fighters.AddRange(monsters);
        fighters.AddRange(heroes);
        fighters.Sort((f1, f2)=>f1.GetSpeed().CompareTo(f2.GetSpeed()));

        while (monsters.Count > 0 && heroes.Count > 0)
        {
            Debug.Log($"Before: Heroes: {heroes.Count}, Monsters: {monsters.Count}");
            for (int i = fighters.Count - 1; i >= 0; i--)
            {
                Debug.Log($"{fighters[i]} attacking...");
                // Make sure the monster is still "alive"
                if (fighters[i] is Monster && monsters.Contains(fighters[i].GetComponent<Monster>()) && heroes.Count > 0)
                {
                    fighters[i].Attack(heroes[0]);
                    fighters[i].Die();
                }
                else if (fighters[i] is Hero && heroes.Contains(fighters[i].GetComponent<Hero>()) && monsters.Count > 0)
                {
                    fighters[i].Attack(monsters[0]);
                }
                else
                {
                    break;
                }
                Debug.Log($"During: Heroes: {heroes.Count}, Monsters: {monsters.Count}");
            }
            Debug.Log($"After: Heroes: {heroes.Count}, Monsters: {monsters.Count}");

        }

        yield return new WaitForSeconds(1);
        Debug.Log("Fight Resolved");

        if (heroes.Count == 0)
        {
            Debug.Log("The party has been defeated!");
            PartyManager.GetInstance().DestroyParty();
        }
        else
        {
            Debug.Log("The heroes defeated this room");
            room.HeroesDefeatedMonsters();
        }

    }
}
