using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class DungeonManager : MonoBehaviour
{
    private static DungeonManager instance;

    // If we are currently placing something
    private bool currentlyPlacing;

    // The current thing we are placing
    private RoomBase curRoomBase;
    private MonsterBase curMonsterBase;
    private TrapBase curTrapBase;

    // The placement indicator
    [Header("Placement Indicator Settings")]
    [SerializeField] private GameObject placementIndicator;
    [SerializeField] private float placementIndicatorUpdateRate = 0.05f;
    private float lastUpdateTime;
    // Current position the mouse is and where we would place something
    private Vector3Int curPlacementPos;

    // The tilemap with the room and hallways
    [SerializeField] private Tilemap tilemap;
    
    // Lists that hold all of the rooms and hallways
    private List<Room> rooms = new List<Room>();
    private List<Room> hallways = new List<Room>();
    // Positions of the entrance and boss room
    private Vector3Int entrance;
    private Vector3Int bossRoom;

    [Header("Prefabs for placing a basic dungeon")]
    [SerializeField] private RoomBase hallwayBasePrefab;
    [SerializeField] private RoomBase roomBasePrefab;
    [SerializeField] private RoomBase entranceBasePrefab;
    [SerializeField] private RoomBase bossRoomBasePrefab;

    [Header("Temporary monster for placing a basic dungeon")]
    [SerializeField] private MonsterBase tempMonster;

    private List<RoomBase> roomBases;

    private void Awake() 
    {
        instance = this;
        roomBases = Resources.LoadAll<RoomBase>("").ToList();
        InvokeRepeating("TriggerPeriodicRooms", 1f, 1f);
    }

    public static DungeonManager GetInstance() 
    {
        return instance;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CancelPlacement();
        if (Time.time - lastUpdateTime > placementIndicatorUpdateRate)
            UpdatePlacementIndicator();
        if (currentlyPlacing && Input.GetMouseButtonDown(0))
            Place();
        if (Input.GetKeyDown(KeyCode.Space))
            BeginNewPlacement(entranceBasePrefab);
    }

    public void PlaceImportantRooms()
    {
        BeginNewPlacement(entranceBasePrefab);
    }

    public void EntrancePlaced(Vector3Int pos)
    {
        entrance = pos;
        BeginNewPlacement(bossRoomBasePrefab);
    }

    public void BossRoomPlaced(Vector3Int pos)
    {
        bossRoom = pos;
    }

    private void UpdatePlacementIndicator()
    {
        lastUpdateTime = Time.time;
        curPlacementPos = Selector.instance.GetCurTilePosition();
        placementIndicator.transform.position = curPlacementPos;
    }

    public void BeginNewPlacement(RoomBase roomBase)
    {
        if (!GameManager.GetInstance().HasEnoughMoney(roomBase.GetCost())) return;
        curRoomBase = roomBase;
        BeginNewPlacement();
    }

    public void BeginNewPlacement(MonsterBase monsterBase)
    {
        if (!GameManager.GetInstance().HasEnoughMoney(monsterBase.GetCost())) return;
        foreach (Room r in rooms)
        {
            if (r.CanAddMonster(monsterBase))
                r.Highlight(true);
        }
        curMonsterBase = monsterBase;
        BeginNewPlacement();
    }

    public void BeginNewPlacement(TrapBase trapBase)
    {
        if (!GameManager.GetInstance().HasEnoughMoney(trapBase.GetCost())) return;
        curTrapBase = trapBase;
        BeginNewPlacement();
    }

    public void BeginNewPlacement()
    {
        currentlyPlacing = true;
        placementIndicator.SetActive(true);
    }

    private void CancelPlacement()
    {
        currentlyPlacing = false;
        placementIndicator.SetActive(false);
        curRoomBase = null;
        curMonsterBase = null;
        curTrapBase = null;
        HighlightRooms(false);
    }

    private void Place()
    {
        if (curRoomBase != null) PlaceRoom(curPlacementPos, curRoomBase);
        else if (curMonsterBase != null) PlaceMonster(curPlacementPos, curMonsterBase);
        else if (curTrapBase != null) PlaceTrap(curPlacementPos, curTrapBase);
        else DestroyTile(curPlacementPos);
    }

    private void PlaceRoom(int x, int y, RoomBase roomBase)
    {
        PlaceRoom(new Vector3Int(x, y), roomBase);
    }

    private void PlaceRoom(Vector3Int pos, RoomBase roomBase)
    {
        if (tilemap.GetTile(pos) != null) return;
        if (roomBase != hallwayBasePrefab) CancelPlacement();
        GameManager.GetInstance().SpendMoney(roomBase.GetCost());
        tilemap.SetTile(pos, roomBase.GetTile());
        tilemap.GetInstantiatedObject(pos).GetComponent<Room>().SetType(roomBase);
    }

    private void PlaceMonster(int x, int y, MonsterBase monsterBase)
    {
        PlaceMonster(new Vector3Int(x, y), monsterBase);
    }

    private void PlaceMonster(Vector3Int pos, MonsterBase monsterBase)
    {
        if (tilemap.GetTile(pos) == null || !tilemap.GetInstantiatedObject(pos).GetComponent<Room>().CanAddMonster(monsterBase)) return;
        Room room = tilemap.GetInstantiatedObject(pos).GetComponent<Room>();
        if (room.monsters.Count >= room.monsterCapacity) return;
        room.AddMonster(monsterBase);
        GameManager.GetInstance().SpendMoney(monsterBase.GetCost());
        CancelPlacement();
    }

    private void PlaceTrap(int x, int y, TrapBase trapBase)
    {
        PlaceTrap(new Vector3Int(x, y), trapBase);
    }

    private void PlaceTrap(Vector3Int pos, TrapBase trapBase)
    {
        if (tilemap.GetTile(pos) == null) return;
        Room room = tilemap.GetInstantiatedObject(pos).GetComponent<Room>();
        if (room.traps.Count >= room.trapCapacity) return;
        room.AddTrap(trapBase);
        GameManager.GetInstance().SpendMoney(trapBase.GetCost());
        CancelPlacement();
    }

    private void DestroyTile(Vector3Int pos)
    {
        if (tilemap.GetTile(pos) == null) return;
        // TODO: Change this to a method in room so it refunds some money for the room, monsters and traps
        tilemap.SetTile(pos, null);
        CancelPlacement();
        // Hide because we would be hovering over something and it gets destroyed so we need to stop hovering
        Tooltip.HideTooltip_Static();
    }

    public void PlaceBasicDungeon() {
        
        // Place a basic dungeon like this:
        //     B 
        //     | 
        //     | 
        //  R--+--R
        //     | 
        //     E 

        PlaceRoom(0, 3, bossRoomBasePrefab);

        PlaceRoom(0, 2, hallwayBasePrefab);
        PlaceRoom(0, 1, hallwayBasePrefab);
        PlaceRoom(0, 0, hallwayBasePrefab);
        PlaceRoom(0, -1, hallwayBasePrefab);
        PlaceRoom(-1, 0, hallwayBasePrefab);
        PlaceRoom(1, 0, hallwayBasePrefab);

        PlaceRoom(-2, 0, roomBasePrefab);
        PlaceMonster(-2, 0, tempMonster);
        PlaceMonster(-2, 0, tempMonster);
        PlaceMonster(-2, 0, tempMonster);
        PlaceRoom(2, 0, roomBasePrefab);

        PlaceRoom(0, -2, entranceBasePrefab);
    }

    private void HighlightRooms(bool status) 
    {
        foreach (Room room in rooms)
        {
            room.Highlight(status);
        }
    }

    public void ResetDungeon() 
    {
        foreach (Room room in rooms)
        {
            room.ResetRoom();
        }
        tilemap.GetInstantiatedObject(bossRoom).GetComponent<Room>().ResetRoom();
        // Im not sure if this is the best way?
        foreach (Transform child in FightManager.GetInstance().GetMonsterHolder().transform)
        {
            if (child != FightManager.GetInstance().GetMonsterHolder().transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void TriggerPeriodicRooms()
    {
        foreach (Room r in rooms)
            r.TriggerPeriodic();
    }

    public Vector3Int GetBossRoomPos() {return bossRoom;}
    public Vector2Int GetBossRoomTile() {return new Vector2Int(bossRoom.x, bossRoom.y);}
    public Vector3Int GetEntrancePos() {return entrance;}
    public Vector2Int GetEntranceTile() {return new Vector2Int(entrance.x, entrance.y);}
    public List<Room> GetRooms() {return rooms;}
    public List<Room> GetHallways() {return hallways;}
    public Tilemap GetTilemap() {return tilemap;}
}
