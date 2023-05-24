using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    private static FightManager instance;

    public GameObject monsterPrefab, monsterHolder, heroPrefab, heroHolder, fightViewer;
    public List<Fighter> order;
    public List<Monster> monsters;
    public List<Hero> heroes;
    public Room room;

    [SerializeField] private GameObject portraitPrefab, orderHolder;
    private List<Image> portraits = new List<Image>();
    
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

        foreach (MonsterBase mb in monsterBases)
        {
            AddMonster(mb);
        }

        heroes.AddRange(party);

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
            h.EnterRoom(room);
        }
        order.Sort((f1, f2)=>f2.GetSpeed().CompareTo(f1.GetSpeed()));
        UpdateOrder();

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
            UpdateOrder();
            
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
        Debug.Log($"{f} died");
        if (f is Monster)
        {
            if (monsters.Contains(f.GetComponent<Monster>()))
            {
                monsters.Remove(f.GetComponent<Monster>());
                order.Remove(f);
                Debug.Log($"{f} removed! (m)");
            }
        }
        else if (f is Hero)
        {
            if (heroes.Contains(f.GetComponent<Hero>()))
            {
                heroes.Remove(f.GetComponent<Hero>());
                order.Remove(f);
                Debug.Log($"{f} removed! (h)");
            }
        }
    }

    private void UpdateOrder()
    {
        while (portraits.Count < order.Count)
        {
            AddPortrait();
        }

        foreach (Image i in portraits)
        {
            i.gameObject.SetActive(false);
        }
        
        for (int i = 0; i < order.Count; i++)
        {
            portraits[i].sprite = order[i].GetSprite();
            portraits[i].gameObject.SetActive(true);
        }

        orderHolder.GetComponent<Animator>().SetTrigger("Next");
    }

    private void AddPortrait()
    {
        GameObject gameObject = Instantiate(portraitPrefab, orderHolder.transform);
        portraits.Add(gameObject.GetComponent<Image>());
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
