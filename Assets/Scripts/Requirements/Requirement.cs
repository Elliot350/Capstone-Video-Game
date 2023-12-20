using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Requirement 
{
    public RoomBase room;
    public MonsterBase monster;
    public int requiredRoomCount;
    public int requiredMonsterCount;
    private Color completeColour = Color.green;
    private Color incompleteColour = Color.red;

    public bool IsValid()
    {
        List<Room> rooms = DungeonManager.GetInstance().GetRooms();
        int roomCount = 0;
        int monsterCount = 0;
        foreach (Room r in rooms)
        {
            if (r.GetRoomBase() == room) 
                roomCount++;
            foreach (MonsterBase m in r.GetMonsters())
            {
                if (m == monster)
                    monsterCount++;
            }
            if (monsterCount >= requiredMonsterCount && roomCount >= requiredRoomCount)
                return true;
        }
        
        return false;
    }

    public override string ToString() 
    {
        string text = "";
        bool hasMonsters = false;
        bool hasRooms = false;
        List<Room> rooms = DungeonManager.GetInstance().GetRooms();
        int roomCount = 0;
        int monsterCount = 0;
        foreach (Room r in rooms)
        {
            if (r.GetRoomBase() == room) 
                roomCount++;
            foreach (MonsterBase m in r.GetMonsters())
            {
                if (m == monster)
                    monsterCount++;
            }
            if (monsterCount >= requiredMonsterCount)
                hasMonsters = true;
            if (roomCount >= requiredRoomCount)
                hasRooms = true;
        }
        if (requiredMonsterCount > 0)
            text += "Have <color=#" + ColorUtility.ToHtmlStringRGBA(hasMonsters ? completeColour : incompleteColour) + ">" + requiredMonsterCount + "</color> " + monster.GetName() + (requiredMonsterCount != 1 ? "s" : "") + ".";
        if (requiredRoomCount > 0)
            text += "Have <color=#" + ColorUtility.ToHtmlStringRGBA(hasRooms ? completeColour : incompleteColour) + ">" + requiredRoomCount + "</color> " + room.GetName() + (requiredRoomCount != 1 ? "s" : "") + ".";

        return text;
    }
}