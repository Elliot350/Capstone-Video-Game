using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Requirement 
{
    public RoomBase room;
    public MonsterBase monster;
    public Tag tag;
    public int requiredRoomCount;
    public int requiredMonsterCount;
    public int requiredTagCount;

    private int roomCount;
    private int monsterCount;
    private int tagCount;
    private bool hasRooms;
    private bool hasMonsters;
    private bool hasTags;


    private Color completeColour = Color.green;
    private Color incompleteColour = Color.red;

    public bool IsValid()
    {
        Check();
        return hasRooms && hasMonsters && hasTags;
    }

    public override string ToString() 
    {
        Check();
        string text = "";
    
        if (requiredMonsterCount > 0)
            text += "Have <color=#" + ColorUtility.ToHtmlStringRGBA(hasMonsters ? completeColour : incompleteColour) + ">" + requiredMonsterCount + "</color> " + monster.GetName() + (requiredMonsterCount != 1 ? "s" : "") + ". (" + monsterCount + "/" + requiredMonsterCount + ")";
        if (requiredRoomCount > 0)
            text += (text.Length > 0 ? "\n" : "") + "Have <color=#" + ColorUtility.ToHtmlStringRGBA(hasRooms ? completeColour : incompleteColour) + ">" + requiredRoomCount + "</color> " + room.GetName() + (requiredRoomCount != 1 ? "s" : "") + ". (" + roomCount + "/" + requiredRoomCount + ")";
        if (requiredTagCount > 0)
            text += (text.Length > 0 ? "\n" : "") + "Have <color=#" + ColorUtility.ToHtmlStringRGBA(hasTags ? completeColour : incompleteColour) + ">" + requiredTagCount + "</color> " + tag.Format() + (requiredTagCount != 1 ? " monsters." : " monster.") + " (" + tagCount + "/" + requiredTagCount + ")";

        return text;
    }

    private void Check()
    {
        roomCount = 0;
        monsterCount = 0;
        tagCount = 0;

        // TODO: Decide which is better
        
        // DungeonManager.GetInstance().GetRooms().ForEach((r) => {
        //     if (r.GetRoomBase() == room) roomCount++;
        //     r.GetMonsters().ForEach((m) => {
        //         if (m == monster) monsterCount++;
        //         m.GetTags().ForEach((t) => {
        //             if (t == tag) tagCount++;
        //         });
        //     });
        // });
        foreach (Room r in DungeonManager.GetInstance().GetRooms())
        {
            if (r.GetRoomBase() == room) roomCount++;
            foreach (MonsterBase m in r.GetMonsters())
            {
                if (m == monster) monsterCount++;
                foreach (Tag t in m.GetTags())
                {
                    if (t == tag) tagCount++;
                }
            }
        }

        hasRooms = roomCount >= requiredRoomCount;
        hasMonsters = monsterCount >= requiredMonsterCount;
        hasTags = tagCount >= requiredTagCount;
    }

}