using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoomDescriptionBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomName, roomCost, roomDescription, roomMonsterCapacity, roomTrapCapacity;
    [SerializeField] private Image roomImage;
    [SerializeField] private GameObject hoverBox;
    // [SerializeField] private SpriteRenderer bounds;
    // [SerializeField] private Camera cam;

    public void ShowDescription(RoomBase room)
    {
        roomName.text = room.GetName();
        roomCost.text = room.GetCost().ToString();
        roomDescription.text = room.GetDescription();
        roomMonsterCapacity.text = room.GetMonster().ToString();
        roomTrapCapacity.text = room.GetTrap().ToString();
        roomImage.sprite = room.GetSprite();
        
        // hoverBox.transform.position.Set(GameManager.GetInstance().cam.ScreenToWorldPoint(Input.mousePosition).x, GameManager.GetInstance().cam.ScreenToWorldPoint(Input.mousePosition).y, hoverBox.transform.position.z);
        hoverBox.SetActive(true);
    }

    public void HideDescription()
    {
        hoverBox.SetActive(false);
    }
}
