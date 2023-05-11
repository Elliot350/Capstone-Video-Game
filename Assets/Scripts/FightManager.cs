using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    private static FightManager instance;

    public GameObject monsterPrefab, monsterHolder, heroPrefab, heroHolder;
    public List<Fighter> order;
    public List<Monster> monsters;
    public List<Hero> heroes;

    private WaitForSeconds shortPause = new WaitForSeconds(1);

    private void Awake()
    {
        instance = this;
    }

    public static FightManager GetInstance()
    {
        return instance;
    }

    public IEnumerator StartFight(List<Hero> heroBases, List<MonsterBase> monsterBases, Room room)
    {
        if (heroBases.Count == 0 || monsterBases.Count == 0) 
            yield break;

        order = new List<Fighter>();

        heroes = new List<Hero>();
        monsters = new List<Monster>();

        foreach (MonsterBase mb in monsterBases)
        {
            Monster monster = Instantiate(monsterPrefab, monsterHolder.transform).GetComponent<Monster>();
            monster.SetType(mb, room);
            order.Add(monster);
            monsters.Add(monster);
        }

        // TODO: Change this to make it spawn heroes
        heroes.AddRange(heroBases);
        order.AddRange(heroBases);
        order.Sort((f1, f2)=>f1.GetSpeed().CompareTo(f2.GetSpeed()));

        // ShowFighters(fighters, room);
        foreach (Fighter f in order)
        {
            // f.ShowFighter();
            Debug.Log($"Fighter: {f}");
        }
        
        yield return shortPause;

        int turn = 0;
        while (monsters.Count > 0 && heroes.Count > 0)
        {
            turn++;
            Debug.Log($"Turn: {turn}, Monsters: {monsters.Count}, Heroes: {heroes.Count}");
            // Debug.Log($"Before: Heroes: {heroes.Count}, Monsters: {monsters.Count}");
            for (int i = order.Count - 1; i >= 0; i--)
            {
                Fighter fighter = order[i];
                Debug.Log($"{fighter} attacking...");
                // Make sure the monster is still "alive"
                if (fighter is Monster && heroes.Count > 0)
                {
                    fighter.Attack(heroes);
                }
                else if (fighter is Hero && monsters.Count > 0)
                {
                    fighter.Attack(monsters);
                }
                yield return shortPause;
                fighter.DoneAttack();
                // Debug.Log($"During: Heroes: {heroes.Count}, Monsters: {monsters.Count}");
            }
            if (turn >= 10)
                break;
            // Debug.Log($"After: Heroes: {heroes.Count}, Monsters: {monsters.Count}");

        }

        yield return shortPause;
        Debug.Log("Fight Resolved");

        if (heroBases.Count == 0)
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

    public int CountHeroes()
    {
        int count = 0;
        foreach (Fighter f in order)
        {
            if (f is Hero)
                count++;
        }
        return count;
    }

    public void FighterDied(Fighter f)
    {
        if (f is Monster)
        {
            if (monsters.Contains(f.GetComponent<Monster>()))
            {
                monsters.Remove(f.GetComponent<Monster>());
                order.Remove(f);
            }
        }
        else if (f is Hero)
        {
            if (heroes.Contains(f.GetComponent<Hero>()))
            {
                heroes.Remove(f.GetComponent<Hero>());
                order.Remove(f);
            }
        }
    }

    public void ShowFighters(List<Fighter> fighters, Room room)
    {
        float monsterOffset = 0f;
        float heroOffset = 0f;

        foreach (Fighter f in fighters)
        {
            if (f is Monster)
            {
                // f.ShowFighter(room);
                Debug.Log($"Before: {f.transform.position}, {f.gameObject.transform.position}");
                f.gameObject.transform.position = new Vector3(f.transform.position.x - monsterOffset, f.transform.position.y, f.transform.position.z);
                monsterOffset += 1f;
                Debug.Log($"After: {f.transform.position}, {f.gameObject.transform.position}");
            }
            else if (f is Hero)
            {
                // f.ShowFighter(room);
                f.gameObject.transform.position.Set(f.transform.position.x + heroOffset, f.transform.position.y, f.transform.position.z);
                heroOffset += 1f;
                Debug.Log($"{heroOffset}");
            }
            else
            {
                Debug.LogWarning($"{f} wasn't a hero or a monster!");
            }
        }
    }
}
