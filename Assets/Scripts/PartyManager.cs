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

    public Tilemap tilemap;
    public Tilemap roadMap;
    public TileBase roadTile;
    public Vector3Int[,] spots;
    Astar astar;
    List<Spot> roadPath = new List<Spot>();
    new Camera camera;
    BoundsInt bounds;

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

        tilemap.CompressBounds();
        roadMap.CompressBounds();
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

    private void DrawRoad()
    {
        foreach (Spot path in roadPath)
        {
            roadMap.SetTile(new Vector3Int(path.X, path.Y, 0), roadTile);
        }
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
        Vector3Int nextPosition = Vector3Int.FloorToInt(party.transform.position);
        nextPosition.x += 1;
        if (RoomPlacer.GetInstance().tilemap.GetTile(nextPosition) != null) {
            lastMoveTime = Time.time;
            party.transform.position = nextPosition;
            JustMoved();
            return;
        }

        nextPosition = Vector3Int.FloorToInt(party.transform.position);
        nextPosition.y += 1;
        if (RoomPlacer.GetInstance().tilemap.GetTile(nextPosition) != null) {
            lastMoveTime = Time.time;
            party.transform.position = nextPosition;
            JustMoved();
            return;
        }
    }

    private void JustMoved() {
        GameObject roomGameObject = RoomPlacer.GetInstance().tilemap.GetInstantiatedObject(Vector3Int.FloorToInt(party.transform.position));
        Room room = roomGameObject.GetComponent<Room>();
        room.PartyEntered(party.heroes);
        if (room != null) {
            FightManager.GetInstance().StartFight(party.heroes, room.monsters);
        }
    }
}
