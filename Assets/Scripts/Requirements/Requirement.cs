using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Requirement 
{
    public RoomBase room;
    public MonsterBase monster;
    public int roomCount;
    public int monsterCount;

    public bool IsValid()
    {
        List<Room> rooms = DungeonManager.GetInstance().GetRooms();
        foreach (Room r in rooms)
        {
            if (r.GetRoomBase() == room) 
                roomCount--;
            foreach (MonsterBase m in r.GetMonsters())
            {
                if (m == monster)
                    monsterCount--;
            }
            if (roomCount <= 0 && monsterCount <= 0)
                return true;
        }
        
        return false;
    }
}