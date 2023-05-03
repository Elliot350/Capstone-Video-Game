using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class RoomPlacer : MonoBehaviour
{
    public static RoomPlacer instance;
    
    private bool currentlyPlacing;
    private RoomBase curRoomBase;
    private float placementIndicatorUpdateRate = 0.05f;
    private float lastUpdateTime;
    private Vector3Int curPlacementPos;
    public GameObject placementIndicator;
    public Tilemap tilemap;
    public RuleTile tile;

    [SerializeField]
    private List<RoomBase> roomBases;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        roomBases = Resources.LoadAll<RoomBase>("").ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CancelRoomPlacement();
        if (Time.time - lastUpdateTime > placementIndicatorUpdateRate)
        {
            lastUpdateTime = Time.time;
            curPlacementPos = Selector.instance.GetCurTilePosition();
            placementIndicator.transform.position = curPlacementPos;
        }
        if (currentlyPlacing && Input.GetMouseButtonDown(0))
        {
            PlaceRoom();
        }
    }
    
    public static RoomPlacer GetInstance() {
        return instance;
    }

    public void BeginNewRoomPlacement(RoomBase roomBase)
    {
        if (GameManager.GetInstance().money < roomBase.cost)
            return;
        currentlyPlacing = true;
        curRoomBase = roomBase;
        placementIndicator.SetActive(true);
    }

    public void CancelRoomPlacement()
    {
        currentlyPlacing = false;
        placementIndicator.SetActive(false);
    }

    public void PlaceRoom()
    {
        PlaceRoom(curPlacementPos, curRoomBase);
        CancelRoomPlacement();
    }

    public void PlaceRoom(int x, int y, RoomBase room) {
        PlaceRoom(new Vector3Int(x, y), room);
    }

    public void PlaceRoom(Vector3Int position, RoomBase room)
    {
        if (tilemap.GetTile(position) != null) {
            GameManager.GetInstance().ErrorMessage("Something is already there!");
            return;
        }
        tilemap.SetTile(position, room.tile);
        Debug.Log(room);
        tilemap.GetInstantiatedObject(position).GetComponent<Room>().SetType(room);
        GameManager.GetInstance().SpendMoney(room.cost);
    }
}
