using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FightManager : MonoBehaviour
{
    private static FightManager instance;

    // Prefabs
    [SerializeField] private GameObject monsterPrefab, heroPrefab;
    // Holder for monster and heroes
    [SerializeField] private GameObject monsterHolder, heroHolder;
    // Lists for fights
    [SerializeField] private List<Fighter> order, monsters, heroes;
    // Action list that controlls the fights
    private List<Action> actions;
    // Current room the fight is in
    private Room room; // Could maybe remove this

    [SerializeField] private GameObject portraitPrefab;
    [SerializeField] private GameObject orderHolder;
    private List<Image> portraits = new List<Image>();
    
    private WaitForSeconds shortPause = new WaitForSeconds(0.5f);
    private WaitForSeconds secondPause = new WaitForSeconds(1);

    [SerializeField] private TextMeshProUGUI actionsText;

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
        // If any thing is already empty return
        if (party.Count == 0 || monsterBases.Count == 0) 
            yield break;

        // Initialize the lists
        order = new List<Fighter>();
        heroes = new List<Fighter>();
        monsters = new List<Fighter>();
        actions = new List<Action>();

        room = roomFight;

        foreach (MonsterBase mb in monsterBases)
        {
            AddMonster(mb);
        }

        heroes.AddRange(party);
        
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
            Debug.Log($"Attack #{count}: {fighter} attacking...");
            // Make sure the monster is still "alive"
            if (fighter is Monster && heroes.Count > 0)
            {
                AddAction(new GetTargets(fighter, heroes));
            }
            else if (fighter is Hero && monsters.Count > 0)
            {
                AddAction(new GetTargets(fighter, monsters));
            }

            while (actions.Count > 0)
            {
                Action currentAction = actions[0];
                ShowActions();
                actions.RemoveAt(0);
                yield return StartCoroutine(currentAction.Do());
                yield return secondPause;
                ShowActions();
            }

            // If they are still alive, move them to the end of the order
            if (fighter != null)
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

    private void ShowActions()
    {
        string str = actions.Count.ToString() + ":\n";
        foreach (Action a in actions)
        {
            str += a + "\n";
        }
        actionsText.text = str;
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

    public void AddAction(Action action)
    {
        if (actions.Count > 0)
            actions.Insert(0, action);
        else
            actions.Add(action);
    }

    public GameObject GetMonsterHolder() {return monsterHolder;}
    public GameObject GetHeroHolder() {return heroHolder;}
}

// ---------- Actions ----------

public abstract class Action
{
    public Fighter fighter;
    // private float waitTime = 1f;

    public Action(Fighter fighter) 
    {
        this.fighter = fighter;
    }

    public abstract IEnumerator Do();
    protected void AddAction(Action a) {FightManager.GetInstance().AddAction(a);}
    // public float GetWaitTime() {return waitTime;}
}

public class Attack : Action
{
    private Fighter source, target;

    public Attack(Fighter source, Fighter target) : base (source)
    {
        this.source = source;
        this.target = target;
    }

    public override IEnumerator Do()
    {
        float attackDamage = source.CalculateDamage();
        Damage attack = new Damage(source, target, attackDamage);
        foreach (FighterAbility a in source.GetAbilities())
            a.OnAttack(attack);
        source.AttackAnimation();
        AddAction(new TakeDamage(attack));
        yield break;
    }
}

public class TakeDamage : Action
{
    private Damage attack;

    public TakeDamage(Damage attack) : base(attack.target)
    {
        this.attack = attack;
    }

    public override IEnumerator Do()
    {
        foreach (FighterAbility a in fighter.GetAbilities())
            a.OnTakenDamage(attack);
        fighter.TakeDamage(attack.damage);
        if (fighter.GetHealth() <= 0)
            AddAction(new Die(fighter, attack));
        yield break;
    }
}

public class Heal : Action
{
    private float amount;

    public Heal(Fighter fighter, float amount) : base(fighter)
    {
        this.amount = amount;
    }

    public override IEnumerator Do()
    {
        foreach (FighterAbility a in fighter.GetAbilities())
            a.OnHeal(fighter);
        fighter.Heal(amount);
        yield break;
    }
}

public class RemoveAbility : Action
{
    private FighterAbility ability;

    public RemoveAbility(Fighter fighter, FighterAbility ability) : base(fighter)
    {
        this.fighter = fighter;
        this.ability = ability;
    }

    public override IEnumerator Do()
    {
        if (fighter.GetAbilities().Contains(ability))
            fighter.GetAbilities().Remove(ability);
        yield break;
    }
}

public class AddAbility : Action
{
    private FighterAbility ability;

    public AddAbility(Fighter fighter, FighterAbility ability) : base(fighter)
    {
        this.fighter = fighter;
        this.ability = ability;
    }

    public override IEnumerator Do()
    {
        if (!fighter.GetAbilities().Contains(ability))
            fighter.GetAbilities().Add(ability);
        yield break;
    }
}

public class Die : Action
{
    private Damage attack;

    public Die(Fighter fighter, Damage attack) : base(fighter)
    {
        this.attack = attack;
    }

    public override IEnumerator Do()
    {
        foreach (FighterAbility a in fighter.GetAbilities())
            a.OnDeath(attack);
        FightManager.GetInstance().FighterDied(fighter);
        fighter.DeathAnimation();
        yield break;
    }
}

public class GetTargets : Action
{
    private List<Fighter> fighters;

    public GetTargets(Fighter fighter, List<Fighter> fighters) : base(fighter)
    {
        this.fighters = fighters;
    }

    public override IEnumerator Do()
    {
        List<Fighter> targets = new List<Fighter>();
        targets.Add(fighters[0]);
        foreach (FighterAbility a in fighter.GetAbilities())
        {
            if (a.DecideTargets(fighters).Count >= targets.Count)
                targets = a.DecideTargets(fighters);
        }
        foreach (Fighter f in targets)
            AddAction(new Attack(fighter, f));
        yield break;
    }
}