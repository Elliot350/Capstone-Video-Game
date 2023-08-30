using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    
    [Header("Currencies")]
    [SerializeField] private int money;
    [SerializeField] private int mana;

    [Header("Lists for the rooms, monsters, heroes, traps, etc.")]
    [SerializeField] public RoomBase hallway;
    [SerializeField] public UnityEngine.Tilemaps.TileBase empty;
    [SerializeField] private List<RoomBase> roomBases;
    [SerializeField] private List<MonsterBase> monsterBases;
    [SerializeField] private List<HeroBase> heroBases;
    [SerializeField] private List<TrapBase> trapBases;
    [SerializeField] private List<PartyLayout> partyLayouts;
    private LocationData locationData;
    [SerializeField] private LocationData defaultLocation;

    // public RoomInfo roomInfo;
    // [Header("Camera")]
    // [SerializeField] private Camera cam;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Debug.LogWarning("Extra GameManager in scene!");
            Destroy(gameObject);
        }

        
    }

    // Start is called before the first frame update
    void Start()
    {
        FormatText();
        if (locationData == null) SetLocationData(defaultLocation);
        DungeonManager.GetInstance().PlaceEmpties();
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

        // if (Input.GetKeyDown(KeyCode.G)) {
        //     Debug.Log($"Result: {GetRandomMonster((m) => m.HasTag(tagToSearch))}");
        // }
    }


    public static GameManager GetInstance()
    {
        return instance;
    }

    public void SetLocationData(LocationData data)
    {
        locationData = data;
        hallway = data.hallways;
        empty = data.empty;
        roomBases = new List<RoomBase>(data.rooms);
        monsterBases = new List<MonsterBase>(data.monsters);
        heroBases = new List<HeroBase>(data.heroes);
        trapBases = new List<TrapBase>(data.traps);
        partyLayouts = new List<PartyLayout>(data.partyLayouts);
    }

    public void OnPlaceBuilding(RoomBase room)
    {
        
    }

    public int GetMoney()
    {
        return money;
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

    public int GetMana()
    {
        return mana;
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
        if (UIManager.GetInstance() != null) UIManager.GetInstance().SetText();
    }

    public void RoomClickedOn(Room room)
    {
        // roomInfo.ShowRoom(room);
        
    }

    // public void AddBoss()
    // {
    //     AddBoss(bossMonster);
    // }

    public void AddBoss(MonsterBase monster)
    {
        DungeonManager.GetInstance().GetTilemap().GetInstantiatedObject(DungeonManager.GetInstance().GetBossRoomPos()).GetComponent<Room>().AddMonster(monster);
    }

    public HeroBase GetRandomHero()
    {
        return heroBases[UnityEngine.Random.Range(0, heroBases.Count)];
    }

    public HeroBase GetRandomHero(Func<HeroBase, bool> condition)
    {
        List<HeroBase> possibleHeroes = new List<HeroBase>();

        heroBases.ForEach((heroBase) => {
            if (condition(heroBase))
                possibleHeroes.Add(heroBase);
        });

        // Debug.Log($"Possibilities");
        // possibleHeroes.ForEach((m) => Debug.Log($"{m}"));

        return possibleHeroes.Count > 0 ? possibleHeroes[UnityEngine.Random.Range(0, possibleHeroes.Count)] : null;
    }


    public MonsterBase GetRandomMonster()
    {
        return monsterBases[UnityEngine.Random.Range(0, monsterBases.Count)];
    }

    public MonsterBase GetRandomMonster(Func<MonsterBase, bool> condition)
    {
        List<MonsterBase> possibleMonsters = new List<MonsterBase>();

        monsterBases.ForEach((monsterBase) => {
            if (condition(monsterBase))
                possibleMonsters.Add(monsterBase);
        });

        return possibleMonsters.Count > 0 ? possibleMonsters[UnityEngine.Random.Range(0, possibleMonsters.Count)] : null;
    }

    public PartyLayout GetRandomPartyLayout()
    {
        return partyLayouts[UnityEngine.Random.Range(0, partyLayouts.Count)];
    }
    
}