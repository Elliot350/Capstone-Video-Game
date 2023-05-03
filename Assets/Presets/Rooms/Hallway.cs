using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway : Room
{
    // TODO: Make this inherit from RoomBase (like Entrance and BossRoom)
    protected override void Start()
    {
        DungeonManager.GetInstance().hallways.Add(this);
        highlightBox.SetActive(false);
    }
}
