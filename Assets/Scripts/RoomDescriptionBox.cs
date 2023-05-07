using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomDescriptionBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomName, roomCost, roomDescription, roomMonsterCapacity, roomTrapCapacity;
    [SerializeField] private GameObject hoverBox;
    // [SerializeField] private SpriteRenderer bounds;
    // [SerializeField] private Camera cam;

    public void ShowDesciption(RoomBase room)
    {
        roomName.text = room.GetName();
        roomCost.text = room.GetCost().ToString();
        roomDescription.text = room.GetDescription();
        roomMonsterCapacity.text = room.GetMonster().ToString();
        roomTrapCapacity.text = room.GetTrap().ToString();
        
        // hoverBox.transform.position.Set(GameManager.GetInstance().cam.ScreenToWorldPoint(Input.mousePosition).x, GameManager.GetInstance().cam.ScreenToWorldPoint(Input.mousePosition).y, hoverBox.transform.position.z);
        hoverBox.transform.position = new Vector3(GameManager.GetInstance().cam.ScreenToWorldPoint(Input.mousePosition).x, GameManager.GetInstance().cam.ScreenToWorldPoint(Input.mousePosition).y + 0.5f);
        hoverBox.SetActive(true);
    }

    public void HideDescription()
    {
        hoverBox.SetActive(false);
    }

    // private Vector3 Clamp(Vector3 targetPosition)
    // {
    //     float camHeight = cam.orthographicSize;
    //     float camWidth = cam.orthographicSize * cam.aspect;
        
    //     float minX = (bounds.transform.position.x - bounds.bounds.size.x / 2f) + camWidth;
    //     float maxX = (bounds.transform.position.x + bounds.bounds.size.x / 2f) - camWidth;
    
    //     float minY = (bounds.transform.position.y - bounds.bounds.size.y / 2f) + camHeight;
    //     float maxY = (bounds.transform.position.y + bounds.bounds.size.y / 2f) - camHeight;

    //     return new Vector3(Mathf.Clamp(targetPosition.x, minX, maxX), Mathf.Clamp(targetPosition.y, minY, maxY), -10);

    // }
}
