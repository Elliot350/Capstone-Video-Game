using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Tilemaps;

public class PartyManager : MonoBehaviour
{
    private static PartyManager instance;

    public Party partyPrefab;
    public Hero heroPrefab;
    private Party party;

    [SerializeField]
    private List<HeroPreset> heroPresets;

    public float moveTime;
    private float lastMoveTime;
    private int moveStep;

    public Tilemap tilemap;
    public Tilemap roadMap;
    public TileBase roadTile;
    public Vector3Int[,] spots;
    Astar astar;
    List<Spot> roadPath = new List<Spot>();
    new Camera camera;
    BoundsInt bounds;
    public Vector2Int start;


    private void Awake()
    {
        instance = this;
    }

    public static PartyManager GetInstance()
    {
        return instance;
    }
    
    private void Start()
    {
        heroPresets = Resources.LoadAll<HeroPreset>("").ToList();

        // tilemap.CompressBounds();
        // roadMap.CompressBounds();
        bounds = tilemap.cellBounds;
        camera = Camera.main;

        CreateGrid();
        astar = new Astar(spots, bounds.size.x, bounds.size.y);
    }

    public void CreateGrid()
    {
        spots = new Vector3Int[bounds.size.x, bounds.size.y];
        for (int x = bounds.xMin, i = 0; i < bounds.size.x; x++, i++)
        {
            for (int y = bounds.yMin, j = 0; j < bounds.size.y; y++, j++)
            {
                if (tilemap.HasTile(new Vector3Int(x, y, 0)))
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
        CreateGrid();

        if (roadPath != null && roadPath.Count > 0)
            roadPath.Clear();
        
        List<Vector2Int> rooms = new List<Vector2Int>();
        rooms = GetRoomsInOrder();

        Vector2Int entrancePosition = new Vector2Int((int) DungeonManager.GetInstance().entrance.x, (int) DungeonManager.GetInstance().entrance.y);
        roadPath.AddRange(astar.CreatePath(spots, rooms[0], entrancePosition, 100));

        for (int i = 1; i < rooms.Count; i++)
        {
            roadPath.AddRange(astar.CreatePath(spots, rooms[i], rooms[i - 1], 100));
        }

        Vector2Int bossRoomPosition = new Vector2Int((int) DungeonManager.GetInstance().bossRoom.x, (int) DungeonManager.GetInstance().bossRoom.y);
        roadPath.AddRange(astar.CreatePath(spots, bossRoomPosition, rooms[rooms.Count - 1], 100));

        // roadPath = astar.CreatePath(spots, entrancePosition, bossRoomPosition, 100);
    }

    private List<Vector2Int> GetRoomsInOrder()
    {
        Debug.Log("Getting the rooms in order");
        List<Vector2Int> list = new List<Vector2Int>();

        Vector2Int entrancePosition = new Vector2Int(DungeonManager.GetInstance().entrance.x, DungeonManager.GetInstance().entrance.y);
        
        foreach (Room room in DungeonManager.GetInstance().rooms)
        {
            Vector2Int nextRoomPos = new Vector2Int((int) room.transform.position.x, (int) room.transform.position.y);
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

    

    public Party GetParty()
    {
        return party;
    }

    // Update is called once per frame
    void Update()
    {
        if (party != null && Time.time - lastMoveTime > moveTime) {
            Move();
        }

        
        if (Input.GetKeyDown(KeyCode.W))
        {
            
            Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPos = tilemap.WorldToCell(world);
            roadMap.SetTile(new Vector3Int(gridPos.x, gridPos.y, 0), null);
        }
        // if (Input.GetKeyDown(KeyCode.E))
        // {
        //     Debug.Log("Creating path");
        //     CreateGrid();

        //     Vector3 world = camera.ScreenToWorldPoint(Input.mousePosition);
        //     Vector3Int gridPos = tilemap.WorldToCell(world);
            
        //     if (roadPath != null && roadPath.Count > 0)
        //         roadPath.Clear();

        //     roadPath = astar.CreatePath(spots, start, new Vector2Int(gridPos.x, gridPos.y), 100); // Could probably lower the length
        //     if (roadPath == null)
        //         return;
        //     DrawRoad();
        //     start = new Vector2Int(roadPath[0].X, roadPath[0].Y);
        // }
    }

    public void CreateHero(HeroPreset heroPreset)
    {
        Hero heroTemp = Instantiate(heroPrefab, party.transform);
        heroTemp.SetType(heroPreset);
        party.AddHero(heroTemp);
    }

    // Create a random party
    public void CreateParty()
    {
        CreateParty(new List<HeroPreset>{heroPresets[0]});
    }

    public void CreateParty(List<HeroPreset> list)
    {
        party = Instantiate(partyPrefab, Vector3.zero, Quaternion.identity);
        foreach (HeroPreset hero in list)
        {
            CreateHero(hero);
        }
    }

    private void Move() {

        if (moveStep + 1 >= roadPath.Count)
            return;

        // Next position is move step plus one
        Vector3Int newPosition = new Vector3Int(roadPath[moveStep + 1].X, roadPath[moveStep + 1].Y);
        
        if (RoomPlacer.GetInstance().tilemap.GetTile(newPosition) != null) {
            lastMoveTime = Time.time;
            party.transform.position = newPosition;
            JustMoved();
            moveStep++;
            return;
        }

        Debug.LogWarning("Party is trying to go out of the dungeon");

    }

    private void JustMoved() {
        GameObject roomGameObject = RoomPlacer.GetInstance().tilemap.GetInstantiatedObject(Vector3Int.FloorToInt(party.transform.position));
        Room room = roomGameObject.GetComponent<Room>();
        room.PartyEntered(party.heroes);
        if (room != null) {
            FightManager.GetInstance().StartFight(party.heroes, room.currentMonsters, room);
        }
    }

    public void CompletedDungeon()
    {
        DestroyParty();
    }

    public void DestroyParty()
    {
        Destroy(party);
    }
}
