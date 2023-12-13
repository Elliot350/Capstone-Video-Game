using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PartyManager : MonoBehaviour
{
    private static PartyManager instance;

    private enum PartyState {
        WAITING,
        PARTY_READY,
        PARTY_COOLDOWN,
        CANMOVE,
        MOVE_COOLDOWN,
        FIGHTING
    }

    [Header("The current party")]
    [SerializeField] private Party party;

    [Header("Party state")]
    [SerializeField] PartyState partyState = PartyState.WAITING;
    
    // Prefabs for creating parties and heroes
    [Header("Prefabs")]
    [SerializeField] private Party partyPrefab;
    [SerializeField] private Hero heroPrefab;
    [SerializeField] private PartyStatus partyStatus;
    
    [Header("Debug party set up")]
    [SerializeField] private List<HeroBase> tempParty;

    [Header("Time between moves")]
    [SerializeField] private float moveTime;
    private float lastMoveTime;
    private int moveStep;
    private bool canMove;

    [Header("Time between parties")]
    [SerializeField] private float minPartyTime;
    [SerializeField] private float maxPartyTime;
    [SerializeField] private float partyCooldown;
    private float lastPartySpawnTime;
    [SerializeField] Slider slider;

    [Header("References to Tilemaps")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap roadMap;
    [SerializeField] private TileBase roadTile;

    // Astar stuff
    private Astar astar;
    private Vector3Int[,] spots;
    new Camera camera;
    private BoundsInt bounds;
    
    // The path the party will take
    private List<Spot> roadPath = new List<Spot>();

    // Max length for pathfinding
    private const int PATH_LENGTH = 100;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        // tilemap.CompressBounds();
        // roadMap.CompressBounds();
        bounds = tilemap.cellBounds;
        camera = Camera.main;

        canMove = true;

        CreateGrid();
        astar = new Astar(spots, bounds.size.x, bounds.size.y);
    }

    private void Initialize()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Debug.LogWarning($"Duplicate PartyManager");
            Destroy(gameObject);
        }
    }

    public static PartyManager GetInstance()
    {
        return instance;
    }
    
    public Party GetParty()
    {
        return party;
    }

    void Update()
    {
        switch (partyState)
        {
            case (PartyState.PARTY_READY):
                CreateParty(GameManager.GetInstance().GetRandomPartyLayout());
                break;

            case (PartyState.PARTY_COOLDOWN):
                partyCooldown -= Time.deltaTime;
                slider.value = partyCooldown;
                if (partyCooldown <= 0)
                {
                    partyState = PartyState.PARTY_READY;
                }
                break;

            case (PartyState.CANMOVE):
                StartCoroutine(Move());
                partyState = PartyState.MOVE_COOLDOWN;
                break;

            case (PartyState.MOVE_COOLDOWN):
                if (Time.time - lastMoveTime > moveTime && canMove)
                    partyState = PartyState.CANMOVE;
                break;

            case (PartyState.FIGHTING):
                break;

            default:
                break;
        }

        // if (party == null && Time.time - lastPartySpawnTime > partyCooldown)
        // {
        //     CreateParty(GameManager.GetInstance().GetRandomPartyLayout());
        //     partyCooldown = Random.Range(minPartyTime, maxPartyTime);
        // }

                
        // if (party != null && Time.time - lastMoveTime > moveTime && canMove) 
        // {
        //     StartCoroutine(Move());
        // }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(world);
            roadMap.SetTile(new Vector3Int(gridPos.x, gridPos.y, 0), null);
        }
    }

    private IEnumerator Move() {

        if (moveStep + 1 >= roadPath.Count)
            yield break;

        // Next position is move step plus one
        Vector3Int newPosition = new Vector3Int(roadPath[moveStep + 1].X, roadPath[moveStep + 1].Y);
        
        if (DungeonManager.GetInstance().GetTilemap().GetTile(newPosition) != null) {
            canMove = false;
            party.transform.position = newPosition;
            partyState = PartyState.FIGHTING;
            Room room = DungeonManager.GetInstance().GetTilemap().GetInstantiatedObject(Vector3Int.FloorToInt(party.transform.position)).GetComponent<Room>();
            if (room.GetMonsters().Count > 0 && !room.BeenVisited())
                yield return FightManager.GetInstance().StartCoroutine(FightManager.GetInstance().StartFight(party.heroes, room.GetMonsters(), room));
            if (party != null) partyState = PartyState.MOVE_COOLDOWN;
            lastMoveTime = Time.time;
            moveStep++;
            canMove = true;
        }
        else {
            Debug.LogWarning("Party is trying to go out of the dungeon");
        }
    }

    public void CreateHero(HeroBase heroBase)
    {
        Debug.Log($"Creating Hero {heroBase.GetName()}");
        Hero heroTemp = Instantiate(heroPrefab, FightManager.GetInstance().GetHeroHolder().transform);
        heroTemp.SetBase(heroBase, null);
        party.AddHero(heroTemp);
        // party.AddHero(heroBase);
    }

    public void CreateSetParty()
    {
        CreateParty(tempParty);
    }

    // Create a random party
    public void CreateParty()
    {
        List<HeroBase> heroes = new List<HeroBase>();
        for (int i = 0; i < 3; i++)
        {
            heroes.Add(GameManager.GetInstance().GetRandomHero());
        }
        CreateParty(heroes);
    }

    public void CreateParty(PartyLayout layout)
    {
        List<HeroBase> heroes = new List<HeroBase>();
        foreach (Tag t in layout.GetHeroes())
        {
            // Get a random hero that has the current tag
            heroes.Add(GameManager.GetInstance().GetRandomHero((h) => h.HasTag(t)));
        }
        CreateParty(heroes);
    }

    public void CreateParty(List<HeroBase> list)
    {
        CreateGrid();
        // Debug.Log($"Entrance: {DungeonManager.GetInstance().GetEntrancePos()}, {DungeonManager.GetInstance().GetEntranceTile()}, Boss room: {DungeonManager.GetInstance().GetBossRoomPos()}, {DungeonManager.GetInstance().GetBossRoomTile()}, ");
        // Check that both the entrance and boss room exist, and that there is a valid path between them
        if (DungeonManager.GetInstance().GetEntranceTile() == null || DungeonManager.GetInstance().GetBossRoomTile() == null || astar.CreatePath(spots, DungeonManager.GetInstance().GetEntranceTile(), DungeonManager.GetInstance().GetBossRoomTile(), PATH_LENGTH) == null)
        {
            // Debug.Log($"No path available from entrance to boss room!");
            return;
        }
        party = Instantiate(partyPrefab);
        foreach (HeroBase hero in list)
        {
            CreateHero(hero);
        }

        // If sucessful, get the party ready to move
        partyState = PartyState.MOVE_COOLDOWN;
        lastMoveTime = Time.time;
        canMove = true;
        moveStep = 0;
        roadPath.Clear();
        DungeonManager.GetInstance().ResetDungeon();
        partyStatus.SetParty(party);
    }

    public void CompletedDungeon()
    {
        DestroyParty();
    }

    public void HeroHurt(Hero h)
    {
        partyStatus.SetHeroHealth(h);
    }

    // TODO: Clean this method up
    public void HeroDied(Hero h)
    {
        // partyStatus.RemoveHero(h);
        party.HeroDead(h);
    }

    public void DestroyParty()
    {
        Debug.Log($"Destroying party");
        foreach (Transform child in FightManager.GetInstance().GetHeroHolder().transform)
        {
            if (child != FightManager.GetInstance().GetHeroHolder().transform)
            {
                Destroy(child.gameObject);
            }
        }
        StartPartyCycle();
        Destroy(party.gameObject);
        partyStatus.RemoveAllHeroes();
        party = null;
    }

    public void StartPartyCycle()
    {
        lastPartySpawnTime = Time.time;
        partyCooldown = Random.Range(minPartyTime, maxPartyTime);
        partyState = PartyState.PARTY_COOLDOWN;
    }

    public void StopPartyCycle()
    {
        partyState = PartyState.WAITING;
    }

    public void CreateGrid()
    {
        spots = new Vector3Int[bounds.size.x, bounds.size.y];
        for (int x = bounds.xMin, i = 0; i < bounds.size.x; x++, i++)
        {
            for (int y = bounds.yMin, j = 0; j < bounds.size.y; y++, j++)
            {
                if (tilemap.HasTile(new Vector3Int(x, y, 0)) && tilemap.GetTile(new Vector3Int(x, y, 0)) != GameManager.GetInstance().empty)
                {
                    spots[i, j] = new Vector3Int(x, y, 0);
                }
                else
                {
                    spots[i, j] = new Vector3Int(x, y, 1);
                }
            }
        }
    }

    public void DrawRoad()
    {
        // Debug.Log("Path: ");
        foreach (Spot path in roadPath)
        {
            // Debug.Log(new Vector2(path.X, path.Y));
            roadMap.SetTile(new Vector3Int(path.X, path.Y, 0), roadTile);
        }
    }

    public void GenerateCompletePath()
    {
        tilemap.CompressBounds();
        roadMap.CompressBounds();
        CreateGrid();

        if (roadPath != null && roadPath.Count > 0)
            roadPath.Clear();
        
        List<Vector2Int> rooms = new List<Vector2Int>();
        rooms = GetRoomsInOrder();

        Vector2Int entrancePosition = DungeonManager.GetInstance().GetEntranceTile();
        Vector2Int bossRoomPosition = DungeonManager.GetInstance().GetBossRoomTile();
        
        if (rooms.Count == 0)
        {
            roadPath.AddRange(astar.CreatePath(spots, bossRoomPosition, entrancePosition, PATH_LENGTH));
        }
        else
        {
            roadPath.AddRange(astar.CreatePath(spots, rooms[0], entrancePosition, PATH_LENGTH));

            for (int i = 1; i < rooms.Count; i++)
            {
                roadPath.AddRange(astar.CreatePath(spots, rooms[i], rooms[i - 1], PATH_LENGTH));
            }

            roadPath.AddRange(astar.CreatePath(spots, bossRoomPosition, rooms[rooms.Count - 1], PATH_LENGTH));
        }

        // Remove duplicate positions
        for (int i = roadPath.Count - 1; i > 0; i--)
        {
            if (roadPath[i].isEqual(roadPath[i - 1])) {
                roadPath.RemoveAt(i);
            }
        }

    }

    private List<Vector2Int> GetRoomsInOrder()
    {
        Debug.Log("Getting the rooms in order");
        List<Vector2Int> list = new List<Vector2Int>();

        Vector2Int entrancePosition = new Vector2Int(DungeonManager.GetInstance().GetEntrancePos().x, DungeonManager.GetInstance().GetEntrancePos().y);
        
        foreach (Room room in DungeonManager.GetInstance().GetRooms())
        {
            Vector2Int nextRoomPos = new Vector2Int((int) room.transform.position.x, (int) room.transform.position.y);
            // Skip over this room if there is no path
            if (astar.CreatePath(spots, entrancePosition, nextRoomPos, PATH_LENGTH) == null)
                continue;
            bool placed = false;
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if (Mathf.Abs(Vector2Int.Distance(nextRoomPos, entrancePosition)) < Mathf.Abs(Vector2Int.Distance(list[i], entrancePosition)))
                    {
                        list.Insert(i, nextRoomPos);
                        placed = true;
                        break;
                    }
                }
            }
            if (!placed)
                list.Add(nextRoomPos);
        }
        return list;
    }
}