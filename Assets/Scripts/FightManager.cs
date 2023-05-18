using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    private static FightManager instance;

    public GameObject monsterPrefab, monsterHolder, heroPrefab, heroHolder, fightViewer;
    public List<Fighter> order;
    public List<Monster> monsters;
    public List<Hero> heroes;
    public Room room;

    private List<SpriteRenderer> portraits;
    

    private WaitForSeconds shortPause = new WaitForSeconds(0.5f);
    private WaitForSeconds secondPause = new WaitForSeconds(1);

    private void Awake()
    {
        instance = this;
    }

    public static FightManager GetInstance()
    {
        return instance;
    }

    public IEnumerator StartFight(List<Hero> party, List<MonsterBase> monsterBases, Room roomFight)
    {
        if (party.Count == 0 || monsterBases.Count == 0) 
            yield break;

        order = new List<Fighter>();

        heroes = new List<Hero>();
        monsters = new List<Monster>();
        room = roomFight;

        // TODO: Use AddMonster instead
        foreach (MonsterBase mb in monsterBases)
        {
            Monster monster = Instantiate(monsterPrefab, monsterHolder.transform).GetComponent<Monster>();
            monster.SetType(mb, room);
            order.Add(monster);
            monsters.Add(monster);
        }

        // foreach (HeroBase hb in heroBases)
        // {
        //     Hero hero = Instantiate(heroPrefab, heroHolder.transform).GetComponent<Hero>();
        //     hero.SetType(hb, room);
        //     order.Add(hero);
        //     heroes.Add(hero);
        // }
        
        foreach (Hero h in party)
        {
            order.Add(h);
        }
        order.Sort((f1, f2)=>f2.GetSpeed().CompareTo(f1.GetSpeed()));

        UIManager.GetInstance().OpenFightMenu();

        // ShowFighters(fighters, room);
        foreach (Fighter f in order)
        {
            // f.ShowFighter();
            Debug.Log($"Fighter: {f}");
        }
        
        yield return secondPause;
        yield return secondPause;

        int count = 0;
        // Each loop is one fighter attacking
        while (monsters.Count > 0 && party.Count > 0)
        {
            count++;

            Fighter fighter = order[0];
            Debug.Log($"Attack #{count}: {fighter} attacking...");
            // Make sure the monster is still "alive"
            if (fighter is Monster && party.Count > 0)
            {
                fighter.Attack(party);
            }
            else if (fighter is Hero && monsters.Count > 0)
            {
                fighter.Attack(monsters);
            }

            yield return shortPause;
            // fighter.DoneAttack();
            yield return shortPause;

            // Move them to the end of the order
            order.RemoveAt(0);
            order.Add(fighter);
            
            if (count > 100)
                break;
        }

        yield return secondPause;
        Debug.Log("Fight Resolved");

        if (party.Count == 0)
        {
            Debug.Log("The party has been defeated!");
            PartyManager.GetInstance().DestroyParty();
        }
        else
        {
            Debug.Log("The heroes defeated this room");
            room.HeroesDefeatedMonsters();
        }

        UIManager.GetInstance().CloseAllMenus();

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

    private void UpdateOrder()
    {

    }

    public void AddMonster(MonsterBase monsterBase)
    {
        Monster monster = Instantiate(monsterPrefab, monsterHolder.transform).GetComponent<Monster>();
        monster.SetType(monsterBase, room);
        order.Add(monster);
        monsters.Add(monster);
    }

    // TODO: Add a Add Hero method maybe?
}
