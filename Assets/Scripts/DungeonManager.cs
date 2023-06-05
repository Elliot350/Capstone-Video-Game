using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class DungeonManager : MonoBehaviour
{
    private static DungeonManager instance;

    private bool currentlyPlacing;
    private RoomBase curRoomBase;
    private MonsterBase curMonsterBase;
    private TrapBase curTrapBase;
    private float placementIndicatorUpdateRate = 0.05f;
    private float lastUpdateTime;
    private Vector3Int curPlacementPos;
    [SerializeField] private GameObject placementIndicator;
    [SerializeField] private Tilemap tilemap;
    
    private List<Room> rooms = new List<Room>();
    private List<Room> hallways = new List<Room>();
    private Vector3Int entrance;
    private Vector3Int bossRoom;

    [SerializeField] private RoomBase hallwayBase;
    [SerializeField] private RoomBase roomBase;
    [SerializeField] private RoomBase entranceBase;
    [SerializeField] private RoomBase bossRoomBase;
    [SerializeField] private MonsterBase monster;

    private List<RoomBase> roomBases;

    private void Awake() 
    {
        instance = this;
        roomBases = Resources.LoadAll<RoomBase>("").ToList();
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
            BeginNewPlacement(entranceBase);
    }

    public void PlaceImportantRooms()
    {
        BeginNewPlacement(entranceBase);
    }

    public void EntrancePlaced(Vector3Int pos)
    {
        entrance = pos;
        BeginNewPlacement(bossRoomBase);
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
        if (GameManager.GetInstance().money < roomBase.GetCost()) return;
        currentlyPlacing = true;
        curRoomBase = roomBase;
        placementIndicator.SetActive(true);
    }

    public void BeginNewPlacement(MonsterBase monsterBase)
    {
        if (GameManager.GetInstance().money < monsterBase.GetCost()) return;
        foreach (Room r in rooms)
        {
            if (r.CanAddMonster(monsterBase))
                r.Highlight(true);
        }
        currentlyPlacing = true;
        curMonsterBase = monsterBase;
        placementIndicator.SetActive(true);
    }

    public void BeginNewPlacement(TrapBase trapBase)
    {
        if (GameManager.GetInstance().money < trapBase.GetCost()) return;
        currentlyPlacing = true;
        curTrapBase = trapBase;
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
        else Debug.LogWarning("Can't place anything!");
    }

    private void PlaceRoom(int x, int y, RoomBase roomBase)
    {
        PlaceRoom(new Vector3Int(x, y), roomBase);
    }

    private void PlaceRoom(Vector3Int pos, RoomBase roomBase)
    {
        if (tilemap.GetTile(pos) != null) return;
        CancelPlacement();
        tilemap.SetTile(pos, roomBase.GetTile());
        tilemap.GetInstantiatedObject(pos).GetComponent<Room>().SetType(roomBase);
        GameManager.GetInstance().SpendMoney(roomBase.GetCost());
    }

    private void PlaceMonster(int x, int y, MonsterBase monsterBase)
    {
        PlaceMonster(new Vector3Int(x, y), monsterBase);
    }

    private void PlaceMonster(Vector3Int pos, MonsterBase monsterBase)
    {
        if (tilemap.GetTile(pos) == null) return;
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

    public void PlaceBasicDungeon() {

        GameManager.GetInstance().money += 300;

        PlaceRoom(0, 3, bossRoomBase);

        PlaceRoom(0, 2, hallwayBase);
        PlaceRoom(0, 1, hallwayBase);
        PlaceRoom(0, 0, hallwayBase);
        PlaceRoom(0, -1, hallwayBase);
        PlaceRoom(-1, 0, hallwayBase);
        PlaceRoom(1, 0, hallwayBase);

        PlaceRoom(-2, 0, roomBase);
        PlaceMonster(-2, 0, monster);
        PlaceMonster(-2, 0, monster);
        PlaceMonster(-2, 0, monster);
        PlaceRoom(2, 0, roomBase);

        PlaceRoom(0, -2, entranceBase);
    }

    public void HighlightRooms(bool status) {
        foreach (Room room in rooms)
        {
            room.Highlight(status);
        }
    }

    public void ResetDungeon() {
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

    public Vector3Int GetBossRoom() {return bossRoom;}
    public Vector3Int GetEntrance() {return entrance;}
    public List<Room> GetRooms() {return rooms;}
    public List<Room> GetHallways() {return hallways;}
    public Tilemap GetTilemap() {return tilemap;}
}
