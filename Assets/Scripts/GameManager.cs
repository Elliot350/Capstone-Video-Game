using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    
    [Header("Currencies")]
    [SerializeField] private int money;
    [SerializeField] private int mana;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI manaText;

    [Header("Lists for all types of monsters, trap, heroes and rooms")]
    [SerializeField] private List<RoomBase> roomBases = new List<RoomBase>();
    [SerializeField] private List<MonsterBase> monsterBases = new List<MonsterBase>();
    [SerializeField] private List<HeroBase> heroBases = new List<HeroBase>();
    [SerializeField] private List<TrapBase> trapBases = new List<TrapBase>();

    // public RoomInfo roomInfo;
    [Header("Camera")]
    [SerializeField] private Camera cam;

    public MonsterBase bossMonster;

    [Header("Debug stuff")]
    [SerializeField] private Tag tagToSearch;

    void Awake()
    {
        instance = this;
        roomBases = Resources.LoadAll<RoomBase>("").ToList();
        monsterBases = Resources.LoadAll<MonsterBase>("").ToList();
        heroBases = Resources.LoadAll<HeroBase>("").ToList();
        trapBases = Resources.LoadAll<TrapBase>("").ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        FormatText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            GainMoney(100);
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            DungeonManager.GetInstance().PlaceBasicDungeon();
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            // FightManager.GetInstance().StartFight(PartyManager.GetInstance().party.heroes, tempRoom.monsters);
        }

        if (Input.GetKeyDown(KeyCode.G)) {
            Debug.Log($"Result: {GetRandomMonster(tagToSearch)}");
        }
    }


    public static GameManager GetInstance()
    {
        return instance;
    }

    public void OnPlaceBuilding(RoomBase room)
    {
        
    }

    public bool HasEnoughMoney(int amount)
    {
        return money >= amount;
    }

    public void GainMoney(int amount)
    {
        money += amount;
        FormatText();
    }

    public void SpendMoney(int amount)
    {
        money -= amount;
        FormatText();
    }

    public bool HasEnoughMana(int amount)
    {
        return mana >= amount;
    }

    public void GainMana(int amount)
    {
        mana += amount;
        FormatText();
    }

    public void SpendMana(int amount)
    {
        mana -= amount;
        FormatText();
    }

    public void FormatText()
    {
        moneyText.text = $"${money}";
        manaText.text = $"{mana}M";
    }

    public void RoomClickedOn(Room room)
    {
        // roomInfo.ShowRoom(room);
    }

    public void AddBoss()
    {
        DungeonManager.GetInstance().GetTilemap().GetInstantiatedObject(DungeonManager.GetInstance().GetBossRoomPos()).GetComponent<Room>().AddMonster(bossMonster);
    }

    public void AddBoss(MonsterBase monster)
    {
        DungeonManager.GetInstance().GetTilemap().GetInstantiatedObject(DungeonManager.GetInstance().GetBossRoomPos()).GetComponent<Room>().AddMonster(monster);
    }

    public HeroBase GetRandomHero()
    {
        return heroBases[Random.Range(0, heroBases.Count)];
    }

    // public HeroBase GetRandomHero(List<HeroBase> excludedHeroes)
    // {
    //     HeroBase selected;
    //     int count = 0;
    //     do 
    //     {
    //         selected = GetRandomHero();
    //         count++;
    //     } while (!excludedHeroes.Contains(selected) && count < 10);
    //     return selected;
    // }

    public HeroBase GetRandomHero(HeroBase excludedHero)
    {
        // Minus 1 because there is one heroBase excluded
        int index = Random.Range(0, heroBases.Count - 1);
        return heroBases[index <= heroBases.IndexOf(excludedHero) ? index : index + 1];
    }

    public MonsterBase GetRandomMonster()
    {
        return monsterBases[Random.Range(0, monsterBases.Count)];
    }

    public MonsterBase GetRandomMonster(MonsterBase excludedMonster)
    {
        // Minus 1 because there is one monsterBase excluded
        int index = Random.Range(0, monsterBases.Count - 1);
        return monsterBases[index <= monsterBases.IndexOf(excludedMonster) ? index : index + 1];
    }

    // public MonsterBase GetRandomMonster(Tag tag)
    // {
    //     List<MonsterBase> possibleMonsters = new List<MonsterBase>();


    //     monsterBases.ForEach((monsterBase) => {
    //         if (monsterBase.HasTag(tag))
    //             possibleMonsters.Add(monsterBase);
    //     });

    //     return possibleMonsters.Count > 0 ? possibleMonsters[Random.Range(0, possibleMonsters.Count)] : null;
    // }

    public MonsterBase GetRandomMonster(Tag tag)
    {
        // I want to do it like
        // GetRandomMonster(Func<MonsterBase, bool> condition)

        // Then?
        // monsterBases.Select(condition)

        // Func<string, string> selector = str => str.ToUpper();
        List<MonsterBase> possibleMonsters = new List<MonsterBase>();

        // var monsters = monsterBases.Select(p => p.GetCost());
        // Debug.Log($"{monsters}");

        monsterBases.ForEach((monsterBase) => {
            if (monsterBase.HasTag(tag))
                possibleMonsters.Add(monsterBase);
        });

        return possibleMonsters.Count > 0 ? possibleMonsters[Random.Range(0, possibleMonsters.Count)] : null;
    }

    
}