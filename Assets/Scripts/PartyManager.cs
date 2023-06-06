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

    [SerializeField] private Party party;
    [SerializeField] private PartyStatus partyStatus;
    [SerializeField] private List<HeroBase> heroBases;
    [SerializeField] private List<HeroBase> tempParty;

    public float moveTime;
    private float lastMoveTime;
    private int moveStep;
    private bool canMove;

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
    
    public Party GetParty()
    {
        return party;
    }

    private void Start()
    {
        heroBases = Resources.LoadAll<HeroBase>("").ToList();

        // tilemap.CompressBounds();
        // roadMap.CompressBounds();
        bounds = tilemap.cellBounds;
        camera = Camera.main;

        canMove = true;

        CreateGrid();
        astar = new Astar(spots, bounds.size.x, bounds.size.y);
    }

    void Update()
    {
        if (party != null && Time.time - lastMoveTime > moveTime && canMove) {
            StartCoroutine(Move());
        }

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
            Room room = DungeonManager.GetInstance().GetTilemap().GetInstantiatedObject(Vector3Int.FloorToInt(party.transform.position)).GetComponent<Room>();
            yield return room.StartCoroutine(room.PartyEntered(party));
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
        Hero heroTemp = Instantiate(heroPrefab, FightManager.GetInstance().GetHeroHolder().transform);
        heroTemp.SetType(heroBase, null);
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
        CreateParty(new List<HeroBase>{heroBases[0]});
    }

    public void CreateParty(List<HeroBase> list)
    {
        party = Instantiate(partyPrefab);
        foreach (HeroBase hero in list)
        {
            CreateHero(hero);
        }
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

    public void HeroDied(Hero h)
    {
        partyStatus.RemoveHero(h);
        party.HeroDead(h);
    }

    public void DestroyParty()
    {
        foreach (Transform child in FightManager.GetInstance().GetHeroHolder().transform)
        {
            if (child != FightManager.GetInstance().GetHeroHolder().transform)
            {
                Destroy(child.gameObject);
            }
        }
        Destroy(party.gameObject);
        partyStatus.RemoveAllHeroes();
        party = null;
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

        tilemap.CompressBounds();
        roadMap.CompressBounds();
        CreateGrid();

        if (roadPath != null && roadPath.Count > 0)
            roadPath.Clear();
        
        List<Vector2Int> rooms = new List<Vector2Int>();
        rooms = GetRoomsInOrder();

        Vector2Int entrancePosition = new Vector2Int((int) DungeonManager.GetInstance().GetEntrance().x, (int) DungeonManager.GetInstance().GetEntrance().y);
        roadPath.AddRange(astar.CreatePath(spots, rooms[0], entrancePosition, 100));

        for (int i = 1; i < rooms.Count; i++)
        {
            roadPath.AddRange(astar.CreatePath(spots, rooms[i], rooms[i - 1], 100));
        }

        Vector2Int bossRoomPosition = new Vector2Int((int) DungeonManager.GetInstance().GetBossRoom().x, (int) DungeonManager.GetInstance().GetBossRoom().y);
        roadPath.AddRange(astar.CreatePath(spots, bossRoomPosition, rooms[rooms.Count - 1], 100));

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

        Vector2Int entrancePosition = new Vector2Int(DungeonManager.GetInstance().GetEntrance().x, DungeonManager.GetInstance().GetEntrance().y);
        
        foreach (Room room in DungeonManager.GetInstance().GetRooms())
        {
            Vector2Int nextRoomPos = new Vector2Int((int) room.transform.position.x, (int) room.transform.position.y);
            // Skip over this roo if there is no path
            if (astar.CreatePath(spots, entrancePosition, nextRoomPos, 100) == null)
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
