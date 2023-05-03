using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    private static DungeonManager instance;

    public List<Room> rooms = new List<Room>();
    public List<Room> hallways = new List<Room>();
    public Vector3Int entrance;
    public Vector3Int bossRoom;

    public RoomBase hallwayBase;
    public RoomBase roomBase;
    public RoomBase entranceBase;
    public RoomBase bossRoomBase;

    private void Awake() {
        instance = this;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        
    }

    public static DungeonManager GetInstance() {
        return instance;
    }

    public void PlaceBasicDungeon() {

        GameManager.GetInstance().money += 300;

        RoomPlacer.GetInstance().PlaceRoom(0, 3, bossRoomBase);

        RoomPlacer.GetInstance().PlaceRoom(0, 2, hallwayBase);
        RoomPlacer.GetInstance().PlaceRoom(0, 1, hallwayBase);
        RoomPlacer.GetInstance().PlaceRoom(0, 0, hallwayBase);
        RoomPlacer.GetInstance().PlaceRoom(0, -1, hallwayBase);
        RoomPlacer.GetInstance().PlaceRoom(-1, 0, hallwayBase);
        RoomPlacer.GetInstance().PlaceRoom(1, 0, hallwayBase);

        RoomPlacer.GetInstance().PlaceRoom(-2, 0, roomBase);
        RoomPlacer.GetInstance().PlaceRoom(2, 0, roomBase);

        RoomPlacer.GetInstance().PlaceRoom(0, -2, entranceBase);
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
    }
    
}
