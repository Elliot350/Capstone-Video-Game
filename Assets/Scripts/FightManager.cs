using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightManager : MonoBehaviour
{
    private static FightManager instance;

    // Prefabs
    [Header("Prefabs")]
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameObject heroPrefab;
    [SerializeField] private GameObject portraitPrefab;

    // Holder for monsters, heroes and the whole order
    [Header("Holders")]
    [SerializeField] private GameObject monsterHolder;
    [SerializeField] private GameObject heroHolder;
    [SerializeField] private GameObject orderHolder;
    [SerializeField] private GameObject deadHolder;

    // Lists for fights
    [Header("Lists")]
    [SerializeField] private List<Fighter> order;
    [SerializeField] private List<Fighter> monsters;
    [SerializeField] private List<Fighter> heroes;
    [SerializeField] private List<Fighter> dead;
    // Action list that controlls the fights
    private List<FightAction> actions;
    private List<FightAction> actionsToAdd;
    // Current room the fight is in
    private Room currentRoom;
    // If the current fight is a boss fight
    private bool bossFight = false;
    
    private List<Image> portraits = new List<Image>();
    
    // Time pauses for fights
    private WaitForSeconds shortPause = new WaitForSeconds(0.5f);
    private WaitForSeconds secondPause = new WaitForSeconds(1);

    [SerializeField] private bool fastForward;
    [SerializeField] private GameObject pointer;

    // Temporary debug text
    [Header("Temporary debug text")]
    [SerializeField] private TextMeshProUGUI actionsText;

    private void Awake() {instance = this;}

    public static FightManager GetInstance() {return instance;}

    public IEnumerator StartFight(List<Hero> party, List<MonsterBase> monsterBases, Room roomFight)
    {
        // If any thing is already empty return
        if (party.Count == 0 || monsterBases.Count == 0) 
            yield break;

        // Initialize the lists
        order = new List<Fighter>();
        heroes = new List<Fighter>();
        monsters = new List<Fighter>();
        actions = new List<FightAction>();
        actionsToAdd = new List<FightAction>();

        currentRoom = roomFight;

        // Add the monsters to monsters and order, and instantiate them to monster holder
        foreach (MonsterBase mb in monsterBases)
            AddMonster(mb);

        // Add the heroes to the hero list
        heroes.AddRange(party);
        
        // Add the heroes to order and set their room to the current room
        foreach (Hero h in party)
        {
            order.Add(h);
            h.EnterRoom(currentRoom);
        }

        // Sort the fighters by their speed value (TODO: randomize the list before)
        for (int i = 0; i < order.Count; i++)
        {
            int randomIndex = Random.Range(0, order.Count);
            Fighter tmp = order[randomIndex];
            order[randomIndex] = order[i];
            order[i] = tmp;
        }
        UpdateOrder(false);

        // Open the fight menu
        UIManager.GetInstance().OpenFightMenu();
        currentRoom.BattleStart(monsters, heroes);
        
        yield return secondPause;

        // Fail safe, in case there is an infinite loop
        int count = 0;

        bossFight = false;
        foreach (Fighter f in order)
        {
            if (f.isBoss) {
                Debug.Log($"{f} is a boss");
                BossManager.GetInstance().ApplyBuffs(f);
                bossFight = true;
            }
            else {
                Debug.Log($"{f} is not a boss");
            }
        }

        foreach (Fighter f in order)
        {
            AddAction(new BattleStart(f));
        }

        yield return StartCoroutine(PerformActions());

        // Each loop is one fighter attacking, with all the actions resolved
        while (monsters.Count > 0 && heroes.Count > 0)
        {
            count++;

            Fighter currentFighter = order[0];
            Debug.Log($"Turn #{count}: {currentFighter.GetName()} ({currentFighter.GetType()})...");

            AddAction(new Turn(currentFighter));

            // Resolve all of the actions
            yield return StartCoroutine(PerformActions());

            // If they are still alive, move them to the end of the order
            if (!dead.Contains(currentFighter))
            {
                order.Remove(currentFighter);
                order.Add(currentFighter);
            }
            
            yield return shortPause;
            UpdateOrder(true);
            
            if (count > 100)
                break;
        }

        foreach (Fighter f in order)
        {
            AddAction(new BattleEnd(f));
        }

        yield return StartCoroutine(PerformActions());

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
            currentRoom.HeroesDefeatedMonsters();
        }

        // Clear the lists and close the menu
        FinishBattle();
    }

    private IEnumerator PerformActions()
    {        
        CatchUpActions();
        CalculateStats();
        while (actions.Count > 0)
        {
            // I don't think this is necessary
            // CalculateStats();
            FightAction currentAction = actions[0];
            ShowActions();
            actions.RemoveAt(0);
            // Each action has its own validation
            if (!currentAction.IsValid())
                continue;
            if (currentAction.fighter != null) pointer.transform.position = currentAction.fighter.transform.position;
            // Perform the action
            currentAction.Do();
            // If the action needs recalculating, do it (the ones that don't are visual)
            if (currentAction.NeedsCalculation()) CalculateStats();
            // Wait
            yield return new WaitForSeconds(currentAction.GetWaitTime() / (fastForward ? 4f : 1f));
            CatchUpActions();
            ShowActions();
        }
        yield break;
    }
    
    public Monster AddMonster(MonsterBase monsterBase)
    {
        if (monsterBase.GetType() == typeof(BossBase))
        {
            Monster bossMonster = Instantiate(bossPrefab, monsterHolder.transform).GetComponent<Monster>();
            bossMonster.SetBase(monsterBase, currentRoom);
            order.Add(bossMonster);
            monsters.Add(bossMonster);
            return bossMonster;
        }
        else 
        {
            Monster monster = Instantiate(monsterPrefab, monsterHolder.transform).GetComponent<Monster>();
            // MonsterBase newMonster = Instantiate<MonsterBase>(monsterBase, monsterHolder.transform);
            // Debug.Log(newMonster);
            monster.SetBase(monsterBase, currentRoom);
            
            order.Add(monster);
            monsters.Add(monster);
            return monster;
        }
    }

    public Fighter AddFighter(FighterBase fighterBase, int position)
    {
        Fighter fighter;
        if (fighterBase is MonsterBase)
        {
            fighter = Instantiate(monsterPrefab, monsterHolder.transform).GetComponent<Fighter>();
            if (position == -1 || position >= monsters.Count) monsters.Add(fighter);
            else monsters.Insert(position, fighter);
        }
        else if (fighterBase is HeroBase)
        {
            fighter = Instantiate(heroPrefab, heroHolder.transform).GetComponent<Fighter>();
            if (position == -1 || position >= heroes.Count) heroes.Add(fighter);
            else heroes.Insert(position, fighter);
        }
        else 
        {
            Debug.Log($"Error! Couldn't create fighter {fighterBase}/{fighterBase.GetName()}");
            return null;
        }
        if (position != -1) fighter.transform.SetSiblingIndex(position);
        fighter.SetBase(fighterBase, currentRoom);
        order.Add(fighter);

        return fighter;
    }

    public Fighter SummonFighter(FighterBase fighterBase, int position)
    {
        Fighter fighter = AddFighter(fighterBase, position);
        currentRoom.FighterSummoned(fighter);
        foreach (Fighter f in order)
            f.FighterSummoned(fighter);
        return fighter;
    }

    public void MoveFighter(Fighter fighter, int newPosition)
    {
        // TODO: Check this logic
        if (fighter.isMonster)
        {
            monsters.Remove(fighter);
            if (newPosition == -1 || newPosition >= monsters.Count) monsters.Add(fighter);
            else monsters.Insert(newPosition, fighter);
            fighter.transform.SetSiblingIndex(newPosition == -1 || newPosition >= monsters.Count ? monsters.Count - 1 : newPosition);
            fighter.Moved();
        }
        else if (!fighter.isMonster && newPosition < heroes.Count)
        {
            heroes.Remove(fighter);
            if (newPosition == -1 || newPosition >= heroes.Count) heroes.Add(fighter);
            else heroes.Insert(newPosition, fighter);
            fighter.transform.SetSiblingIndex(newPosition == -1 || newPosition >= heroes.Count ? heroes.Count - 1 : newPosition);
            fighter.Moved();
        }
    }

    private void AddPortrait()
    {
        GameObject gameObject = Instantiate(portraitPrefab, orderHolder.transform);
        portraits.Add(gameObject.GetComponent<Image>());
    }

    public void AddAction(FightAction action)
    {
        actionsToAdd.Add(action);
    }

    public void CalculateStats()
    {
        foreach (Fighter f in order)
            f.ResetStats();     
        foreach (Fighter f in order)
            f.CalculateStats();
        currentRoom.CalculateDamage(monsters, heroes);
        currentRoom.CalculateMaxHealth(monsters, heroes);
    }

    private void CatchUpActions()
    {
        if (actions.Count > 0)
            actions.InsertRange(0, actionsToAdd);
        else
            actions.AddRange(actionsToAdd);
        actionsToAdd.Clear();
    }
    
    private void ShowActions()
    {
        string str = actions.Count.ToString() + ":\n";
        foreach (FightAction a in actions)
        {
            str += a + "\n";
        }
        actionsText.text = str;
    }

    public void FighterDied(Fighter f)
    {
        currentRoom.FighterDied(f);
        foreach (Fighter fighter in order)
            fighter.FighterDied(f);
        UpdateOrder(false);
    }

    private void UpdateOrder(bool newTurn)
    {
        // If there isn't enough portraits, add them
        while (portraits.Count < order.Count)
            AddPortrait();

        // Hide all the portraits
        foreach (Image i in portraits)
            i.gameObject.SetActive(false);
        
        // Set each portrait to the corresponding Fighter
        for (int i = 0; i < order.Count; i++)
        {
            portraits[i].sprite = order[i].GetSprite();
            portraits[i].gameObject.SetActive(true);
        }

        if (newTurn)
            orderHolder.GetComponent<Animator>().SetTrigger("Next");
    }

    public void FinishBattle()
    {
        Debug.Log($"Finishing battle");
        Debug.Log($"Was boss fight? {bossFight}");
        Debug.Log($"Monsters: {monsters.Count}, Heroes: {heroes.Count}");
        if (bossFight && heroes.Count == 0)
        {
            Debug.Log($"Boss fight won");
            BossManager.GetInstance().LevelUp();
        }
        else 
        {
            UIManager.GetInstance().CloseAllMenus();
        }
        foreach (Fighter f in dead)
        {
            Destroy(f.gameObject);
        }
        foreach (Fighter f in monsters)
        {
            Destroy(f.gameObject);
        }
        heroes.Clear();
        monsters.Clear();
        order.Clear();
        dead.Clear();
    }

    public List<Fighter> GetAllies(Fighter f) 
    {
        List<Fighter> allies = new List<Fighter>(GetTeam(f));
        allies.Remove(f);
        return allies;
    }
    public List<Fighter> GetTeam(Fighter f) {return f.isMonster ? monsters : heroes;}
    public List<Fighter> GetEnemies(Fighter f) {return f.isMonster ? heroes : monsters;}

    public bool AreEnemies(Fighter f1, Fighter f2)
    {
        return ((f1.isMonster && !f2.isMonster) || (!f1.isMonster && f2.isMonster));
    }

    public List<Fighter> GetMonsters() {return monsters;}
    public List<Fighter> GetHeroes() {return heroes;}
    public List<Fighter> GetFighters() {return order;}
    public List<Fighter> GetDead() {return dead;}
    public Room GetRoom() {return currentRoom;}
    public GameObject GetMonsterHolder() {return monsterHolder;}
    public GameObject GetHeroHolder() {return heroHolder;}
    public GameObject GetDeadHolder() {return deadHolder;}
    public bool FastForwarding() {return fastForward;}
    public bool IsBossFight() {return bossFight;}
}