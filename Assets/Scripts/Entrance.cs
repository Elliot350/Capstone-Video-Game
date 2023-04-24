using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entrance : Room
{
    protected override void Start()
    {
        DungeonManager.GetInstance().entrance = Vector3Int.FloorToInt(transform.position);
        highlightBox.SetActive(false);
    }
}
