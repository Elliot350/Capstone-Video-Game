using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomPlacer : MonoBehaviour
{
    public static RoomPlacer instance;
    
    private bool currentlyPlacing;
    private RoomPreset curRoomPreset;
    private float placementIndicatorUpdateRate = 0.05f;
    private float lastUpdateTime;
    private Vector3Int curPlacementPos;
    public GameObject placementIndicator;
    public Tilemap tilemap;
    public RuleTile tile;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {

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

    public void BeginNewRoomPlacement(RoomPreset roomPreset)
    {
        if (GameManager.GetInstance().money < roomPreset.cost)
            return;
        currentlyPlacing = true;
        curRoomPreset = roomPreset;
        placementIndicator.SetActive(true);
    }

    public void CancelRoomPlacement()
    {
        currentlyPlacing = false;
        placementIndicator.SetActive(false);
    }

    public void PlaceRoom()
    {
        // Debug.Log($"Placing " + curRoomPreset.displayName + " at " + curPlacementPos + "...");
        PlaceRoom(curPlacementPos, curRoomPreset);
        
        // if (tilemap.GetTile(curPlacementPos) != null) {
        //     Debug.LogWarning($"Whoops! There is already something there!");
        //     GameManager.getInstance().ErrorMessage("Something is already there!");
        //     CancelRoomPlacement();
        //     return;
        // }

        // tilemap.SetTile(curPlacementPos, curRoomPreset.tile);
        // GameManager.getInstance().OnPlaceBuilding(curRoomPreset);
        CancelRoomPlacement();
    }

    public void PlaceRoom(int x, int y, RoomPreset room) {
        PlaceRoom(new Vector3Int(x, y), room);
    }

    public void PlaceRoom(Vector3Int position, RoomPreset room)
    {
        if (tilemap.GetTile(position) != null) {
            GameManager.GetInstance().ErrorMessage("Something is already there!");
            return;
        }
        tilemap.SetTile(position, room.tile);
        tilemap.GetInstantiatedObject(position).GetComponent<Room>().SetType(room);
        GameManager.GetInstance().SpendMoney(room.cost);
    }
}
