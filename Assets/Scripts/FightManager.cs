using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightManager : MonoBehaviour
{
    private static FightManager instance;

    public GameObject monsterPrefab, monsterHolder, heroPrefab, heroHolder, fightViewer;
    public List<Fighter> order;
    public List<Fighter> monsters;
    public List<Fighter> heroes;
    public Fighter currentTurn;
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
        heroes = new List<Fighter>();
        monsters = new List<Fighter>();
        
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

        room.StartingFight(monsters, heroes);
        UIManager.GetInstance().OpenFightMenu();
        
        yield return secondPause;
        yield return secondPause;

        int count = 0;
        // Each loop is one fighter attacking
        while (monsters.Count > 0 && party.Count > 0)
        {
            count++;

            Fighter fighter = order[0];
            currentTurn = fighter;
            Debug.Log($"Attack #{count}: {fighter} attacking...");
            // Make sure the monster is still "alive"
            if (fighter is Monster && heroes.Count > 0)
            {
                // fighter.Attack(heroes);
                // yield return fighter.StartCoroutine(fighter.StartAttack(heroes));
                fighter.AddAction(new GetTargets(fighter, heroes));
            }
            else if (fighter is Hero && monsters.Count > 0)
            {
                fighter.AddAction(new GetTargets(fighter, monsters));
            }
            
            // Have the fighter actually do their attack
            yield return fighter.StartCoroutine(fighter.TakeTurn());
            Debug.Log(fighter + " done");

            // If they are still alive, move them to the end of the order
            if (fighter.alive)
            {
                order.RemoveAt(0);
                order.Add(fighter);
            }
            
            yield return shortPause;
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

        heroes.Clear();
        monsters.Clear();
        order.Clear();
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
            if (monsters.Contains(f))
            {
                monsters.Remove(f);
                order.Remove(f);
                Debug.Log($"{f} removed! (m)");
            }
            else
            {
                Debug.LogWarning($"Didn't destroy {f}");
            }
        }
        else if (f is Hero)
        {
            if (heroes.Contains(f))
            {
                heroes.Remove(f);
                order.Remove(f);
                PartyManager.GetInstance().HeroDied(f.GetComponent<Hero>());
                Debug.Log($"{f} removed! (h)");
            }
            else
            {
                Debug.LogWarning($"Didn't destroy {f}");
            }
        }
        else
        {
            Debug.LogWarning($"Don't know what {f} is");
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
        Debug.Log($"Creating monster...");
        Monster monster = Instantiate(monsterPrefab, monsterHolder.transform).GetComponent<Monster>();
        Debug.Log($"Setting type");
        monster.SetType(monsterBase, room);
        
        order.Add(monster);
        monsters.Add(monster);
    }

    // TODO: Add a Add Hero method maybe?
}
