using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway : Room
{
    protected override void Start()
    {
        DungeonManager.GetInstance().hallways.Add(this);
        highlightBox.SetActive(false);
    }
}
