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
    [SerializeField] private GameObject heroPrefab;
    [SerializeField] private GameObject portraitPrefab;

    // Holder for monsters, heroes and the whole order
    [Header("Holders")]
    [SerializeField] private GameObject monsterHolder;
    [SerializeField] private GameObject heroHolder;
    [SerializeField] private GameObject orderHolder;

    // Lists for fights
    [Header("Lists")]
    [SerializeField] private List<Fighter> order;
    [SerializeField] private List<Fighter> monsters;
    [SerializeField] private List<Fighter> heroes;
    // Action list that controlls the fights
    private List<FightAction> actions;
    private List<FightAction> actionsToAdd;
    // Current room the fight is in
    private Room room; // Could maybe remove this
    
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

        room = roomFight;

        // Add the monsters to monsters and order, and instantiate them to monster holder
        foreach (MonsterBase mb in monsterBases)
            AddMonster(mb);

        // Add the heroes to the hero list
        heroes.AddRange(party);
        
        // Add the heroes to order and set their room to the current room
        foreach (Hero h in party)
        {
            order.Add(h);
            h.EnterRoom(room);
        }

        // Sort the fighters by their speed value (TODO: randomize the list before)
        order.Sort((f1, f2)=>f2.GetSpeed().CompareTo(f1.GetSpeed()));
        UpdateOrder(false);

        // Open the fight menu
        UIManager.GetInstance().OpenFightMenu();
        room.StartingFight(monsters, heroes);
        
        yield return secondPause;

        // Fail safe, in case there is an infinite loop
        int count = 0;

        foreach (Fighter f in order)
        {
            AddAction(new BattleStart(f));
        }

        // Each loop is one fighter attacking, with all the actions resolved
        while (monsters.Count > 0 && party.Count > 0)
        {
            count++;

            Fighter fighter = order[0];
            Debug.Log($"Attack #{count}: {fighter} attacking...");

            // Make sure there is still something to attack, and add the GetTargets action for the current fighter
            if (fighter is Monster && heroes.Count > 0)
            {
                AddAction(new GetTargets(fighter, heroes));
            }
            else if (fighter is Hero && monsters.Count > 0)
            {
                AddAction(new GetTargets(fighter, monsters));
            }

            // Resolve all of the actions
            CatchUpActions();
            while (actions.Count > 0)
            {
                FightAction currentAction = actions[0];
                ShowActions();
                pointer.transform.position = currentAction.GetFighter().transform.position;
                actions.RemoveAt(0);
                if (order.Contains(currentAction.fighter))
                    currentAction.Do();
                yield return new WaitForSeconds(fastForward ? currentAction.GetWaitTime() / 4f : currentAction.GetWaitTime());
                CatchUpActions();
                ShowActions();
                
            }

            // If they are still alive, move them to the end of the order
            if (fighter != null)
            {
                order.RemoveAt(0);
                order.Add(fighter);
            }
            
            yield return shortPause;
            UpdateOrder(true);
            
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

        // Clear the lists and close the menu
        heroes.Clear();
        monsters.Clear();
        order.Clear();
        UIManager.GetInstance().CloseAllMenus();

    }
    
    public void AddMonster(MonsterBase monsterBase)
    {
        Monster monster = Instantiate(monsterPrefab, monsterHolder.transform).GetComponent<Monster>();
        monster.SetType(monsterBase, room);
        
        order.Add(monster);
        monsters.Add(monster);
    }

    private void AddPortrait()
    {
        GameObject gameObject = Instantiate(portraitPrefab, orderHolder.transform);
        portraits.Add(gameObject.GetComponent<Image>());
    }

    public void AddAction(FightAction action)
    {
        // if (actions.Count > 0)
        //     actions.Insert(0, action);
        // else
        //     actions.Add(action);
        actionsToAdd.Add(action);
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
        Debug.Log($"{f} died");
        if (f is Monster)
        {
            if (monsters.Contains(f))
            {
                monsters.Remove(f);
                order.Remove(f);
                foreach (Fighter fighter in order)
                    fighter.MonsterDied(f);
                Destroy(f.gameObject, fastForward ? 0f : 0.3f);
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
                foreach (Fighter fighter in order)
                    fighter.HeroDied(f);
                Destroy(f.gameObject, fastForward ? 0f : 0.3f);
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

    public List<Fighter> GetAllies(Fighter f) 
    {
        List<Fighter> allies = GetTeam(f);
        allies.Remove(f);
        return allies;
    }
    public List<Fighter> GetTeam(Fighter f) {return f.IsMonster() ? monsters : heroes;}

    public List<Fighter> GetMonsters() {return monsters;}
    public List<Fighter> GetHeroes() {return heroes;}
    public List<Fighter> GetFighters() {return order;}
    public Room GetRoom() {return room;}
    public GameObject GetMonsterHolder() {return monsterHolder;}
    public GameObject GetHeroHolder() {return heroHolder;}
    public bool FastForwarding() {return fastForward;}
}

// ---------- Actions ----------

public abstract class FightAction
{
    public Fighter fighter;
    protected float waitTime;

    public FightAction(Fighter fighter) 
    {
        this.fighter = fighter;
        waitTime = 1f;
    }

    public abstract void Do();
    protected void AddAction(FightAction a) {FightManager.GetInstance().AddAction(a);}
    public float GetWaitTime() {return waitTime;}
    public Fighter GetFighter() {return fighter;}
}

public class GetTargets : FightAction
{
    private List<Fighter> fighters;

    public GetTargets(Fighter fighter, List<Fighter> fighters) : base(fighter)
    {
        this.fighters = fighters;
        waitTime = 0f;
    }

    public override void Do()
    {
        if (!FightManager.GetInstance().GetFighters().Contains(fighter) || fighters.Count == 0)
            return;
        
        foreach (Fighter f in fighter.GetTargets(fighters))
            AddAction(new Attack(fighter, f));
        
    }
}

public class Attack : FightAction
{
    private Fighter target;

    public Attack(Fighter source, Fighter target) : base (source)
    {
        this.target = target;
    }

    public override void Do()
    {
        float attackDamage = fighter.CalculateDamage();
        Damage attack = new Damage(fighter, target, attackDamage);
        foreach (FighterAbility a in fighter.GetAbilities())
            a.OnAttack(attack);
        fighter.AttackAnimation();
        AddAction(new TakeDamage(attack));
    }
}

public class TakeDamage : FightAction
{
    private Damage attack;

    public TakeDamage(Damage attack) : base(attack.Target)
    {
        this.attack = attack;
    }

    public override void Do()
    {
        fighter.TakeDamage(attack);
        if (fighter.GetHealth() <= 0)
            AddAction(new Die(fighter, attack));
    }
}

public class Die : FightAction
{
    private Damage attack;

    public Die(Fighter fighter, Damage attack) : base(fighter)
    {
        this.attack = attack;
    }

    public override void Do()
    {
        fighter.Die(attack);
        FightManager.GetInstance().FighterDied(fighter);
    }
}

public class Heal : FightAction
{
    private float amount;

    public Heal(Fighter fighter, float amount) : base(fighter)
    {
        this.amount = amount;
    }

    public override void Do()
    {
        fighter.Heal(amount);
    }
}

public class RemoveAbility : FightAction
{
    private FighterAbility ability;

    public RemoveAbility(Fighter fighter, FighterAbility ability) : base(fighter)
    {
        this.fighter = fighter;
        this.ability = ability;
        waitTime = 0f;
    }

    public override void Do()
    {
        if (fighter.GetAbilities().Contains(ability))
            fighter.GetAbilities().Remove(ability);
    }
}

public class AddAbility : FightAction
{
    private FighterAbility ability;

    public AddAbility(Fighter fighter, FighterAbility ability) : base(fighter)
    {
        this.fighter = fighter;
        this.ability = ability;
        waitTime = 0f;
    }

    public override void Do()
    {
        if (!fighter.GetAbilities().Contains(ability))
            fighter.GetAbilities().Add(ability);
        ability.OnAdded(fighter);
    }
}

public class BattleStart : FightAction
{
    public BattleStart(Fighter fighter) : base(fighter)
    {
        waitTime = 0f;
    }

    public override void Do()
    {
        fighter.StartBattle();
    }
}

public class PlayAnimation : FightAction
{
    string animationName;

    public PlayAnimation(Fighter fighter, string animationName) : base(fighter)
    {
        this.animationName = animationName;
        waitTime = 0f;
    }

    public override void Do()
    {
        fighter.PlayEffect(animationName);
    }
}

public class ContinueAnimation : FightAction
{
    string animationName;
    Effect effect;

    public ContinueAnimation(Fighter fighter, string animationName, Effect effect) : base(fighter)
    {
        this.animationName = animationName;
        this.effect = effect;
        waitTime = 0f;
    }

    public override void Do()
    {
        fighter.PlayEffect(animationName, effect);
    }
}

public class Morph : FightAction
{
    FighterBase fighterBase;

    public Morph(Fighter fighter, FighterBase fighterBase) : base(fighter)
    {
        this.fighterBase = fighterBase;
    }

    public override void Do()
    {
        fighter.SetType(fighterBase);
    }
}

public class Summon : FightAction
{
    MonsterBase monsterToSummon;

    public Summon(Fighter fighter, MonsterBase summon) : base(fighter)
    {
        this.monsterToSummon = summon;
    }

    public override void Do()
    {
        FightManager.GetInstance().AddMonster(monsterToSummon);
    }
}