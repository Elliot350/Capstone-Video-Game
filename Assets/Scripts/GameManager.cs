using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    
    [Header("Currencies")]
    [SerializeField] private int money;
    [SerializeField] private int mana;
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float health = 100f;

    [Header("Lists for the rooms, monsters, heroes, traps, etc.")]
    [SerializeField] public RoomBase hallway;
    [SerializeField] public UnityEngine.Tilemaps.TileBase empty;
    [SerializeField] private List<RoomBase> roomBases;
    [SerializeField] private List<MonsterBase> monsterBases;
    [SerializeField] private List<HeroBase> heroBases;
    [SerializeField] private List<TrapBase> trapBases;
    [SerializeField] private List<PartyLayout> partyLayouts;
    [SerializeField] private List<FighterAbility> fighterAbilities;
    private LocationData locationData;
    [SerializeField] private LocationData defaultLocation;
    [Header("Scene numbers")]
    [SerializeField] private int mainMenuScene;
    [SerializeField] private int gameScene;
    [SerializeField] private int gameOverScene;

    // public RoomInfo roomInfo;
    // [Header("Camera")]
    // [SerializeField] private Camera cam;

    void Awake()
    {
        PlayerPrefsManager.Load();
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
        SetLocationData(locationData != null ? locationData : defaultLocation);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // FormatText();
        // if (locationData == null) SetLocationData(defaultLocation);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
            GainMoney(100);
        }

        // if (Input.GetKeyDown(KeyCode.B)) {
        //     DungeonManager.GetInstance().PlaceBasicDungeon();
        // }

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
        fighterAbilities = new List<FighterAbility>(Resources.LoadAll<FighterAbility>(""));
        Debug.Log($"Number of abilities: {fighterAbilities.Count}");
    }

    public void SetLocation(LocationData location)
    {
        locationData = location;
    }

    public void PlayButtonPressed()
    {
        SetLocationData(locationData);
        Debug.Log($"Loading scene");
        SceneManager.LoadScene(gameScene);
        Debug.Log($"Done loading");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
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

    public float GetHealth()
    {
        return health;
    }

    public void SetHealth(float value)
    {
        health = value;
        FormatText();
        if (health <= 0) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public float GetMaxHealth()
    {
        return maxHealth;
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

    public FighterAbility GetRandomAbility(Func<FighterAbility, bool> condition)
    {
        List<FighterAbility> possibleAbilities = new();
        fighterAbilities.ForEach((fighterAbility) => {
            if (condition(fighterAbility))
                possibleAbilities.Add(fighterAbility);
        });

        return possibleAbilities.Count > 0 ? possibleAbilities[UnityEngine.Random.Range(0, possibleAbilities.Count)] : null;
    }

    [ContextMenu("ClearPlayerPrefs")]
    public void ClearPlayerPrefs()
    {
        PlayerPrefsManager.Delete();
    }
    
}