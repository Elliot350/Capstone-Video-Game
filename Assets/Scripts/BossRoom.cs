using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : Room
{
    // Start is called before the first frame update
    protected override void Start()
    {
        DungeonManager.GetInstance().bossRoom = Vector3Int.FloorToInt(transform.position);
        highlightBox.SetActive(false);
    }

}
