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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HoverOver()
    {
        GameManager.GetInstance().roomDescriptionBox.ShowDesciption(roomBase);
    }

    public void HoverOut()
    {
        GameManager.GetInstance().roomDescriptionBox.HideDescription();
    }

    public void Clicked()
    {
        RoomPlacer.GetInstance().BeginNewRoomPlacement(roomBase);
    }

}
