using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    private static DungeonManager instance;

    public List<Room> rooms = new List<Room>();
    public List<Hallway> hallways = new List<Hallway>();
    public Vector3Int entrance;

    public RoomPreset hallway;
    public RoomPreset room;
    public RoomPreset entrancePreset;

    private void Awake() {
        instance = this;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.L)) {
            foreach (Room room in rooms)
            {
                room.SpawnMonsters();
            }
        }

        
    }

    public static DungeonManager GetInstance() {
        return instance;
    }

    public void PlaceBasicDungeon() {
        for (int i = -2; i < 3; i++)
        {
            RoomPlacer.GetInstance().PlaceRoom(i, 0, hallway);
        }

        RoomPlacer.GetInstance().PlaceRoom(0, -1, hallway);
        RoomPlacer.GetInstance().PlaceRoom(0, -2, hallway);
        RoomPlacer.GetInstance().PlaceRoom(-1, -2, hallway);
        RoomPlacer.GetInstance().PlaceRoom(-2, -2, hallway);
        
        RoomPlacer.GetInstance().PlaceRoom(-3, 0, room);
        RoomPlacer.GetInstance().PlaceRoom(2, 1, room);
        RoomPlacer.GetInstance().PlaceRoom(-3, -2, entrancePreset);
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
