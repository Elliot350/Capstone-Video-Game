using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TrapPlacer : MonoBehaviour
{
    public static TrapPlacer instance;

    private bool currentlyPlacing;
    private TrapBase curTrapBase;
    private float placementIndicatorUpdateRate = 0.05f;
    private float lastUpdateTime;
    private Vector3Int curPlacementPos;
    public GameObject placementIndicator;

    [SerializeField]
    public Trap trapPrefab;

    public List<TrapBase> trapBases;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        trapBases = Resources.LoadAll<TrapBase>("").ToList();
    }

    public static TrapPlacer GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CancelTrapPlacement();
        if (currentlyPlacing && Time.time - lastUpdateTime > placementIndicatorUpdateRate)
        {
            lastUpdateTime = Time.time;
            curPlacementPos = Selector.instance.GetCurTilePosition();
            placementIndicator.transform.position = curPlacementPos;
        }
        if (currentlyPlacing && Input.GetMouseButtonDown(0))
        {
            PlaceTrap();
        }
            
    }

    public void BeginNewTrapPlacement(TrapBase trapBase)
    {
        if (GameManager.GetInstance().money < trapBase.GetCost())
            return;
        currentlyPlacing = true;
        curTrapBase = trapBase;
        placementIndicator.SetActive(true);
    }

    public void CancelTrapPlacement()
    {
        currentlyPlacing = false;
        placementIndicator.SetActive(false);
        DungeonManager.GetInstance().HighlightRooms(false);
    }
    
    public void PlaceTrap()
    {
        PlaceTrap(curPlacementPos, curTrapBase);
    }

    public void PlaceTrap(int x, int y, TrapBase trapBase)
    {
        PlaceTrap(new Vector3Int(x, y), trapBase);
    }

    public void PlaceTrap(Vector3Int position, TrapBase trapBase)
    {

        if (RoomPlacer.GetInstance().tilemap.GetInstantiatedObject(position).TryGetComponent<Room>(out Room room))
        {
            PlaceTrap(room, trapBase);
        }
        CancelTrapPlacement();

        
    }

    public void PlaceTrap(Room room, TrapBase trapBase)
    {
        if (room.traps.Count < room.trapCapacity)
        {
            room.AddTrap(trapBase);
            GameManager.GetInstance().SpendMoney(trapBase.GetCost());
        }
    }
}
