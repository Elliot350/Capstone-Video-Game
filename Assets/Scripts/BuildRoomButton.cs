using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildRoomButton : MonoBehaviour
{
    public RoomBase roomBase;
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        image.sprite = roomBase.GetSprite();
    }

    public void HoverOver()
    {
       Tooltip.ShowTooltip_Static(roomBase);
    }

    public void HoverOut()
    {
        Tooltip.HideTooltip_Static();
    }

    public void Clicked()
    {
        DungeonManager.GetInstance().BeginNewPlacement(roomBase);
    }

}
