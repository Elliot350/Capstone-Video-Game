using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildRoomButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomName, roomCost, roomDescription, roomMonsterCapacity, roomTrapCapacity;
    [SerializeField] private GameObject hoverBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDesciption(RoomBase room)
    {
        roomName.text = room.displayName;
        roomCost.text = room.cost.ToString();
        roomDescription.text = room.description;
        roomMonsterCapacity.text = room.monsterCapacity.ToString();
        roomTrapCapacity.text = room.trapCapacity.ToString();
        
        // hoverBox.transform.position.Set(GameManager.GetInstance().cam.ScreenToWorldPoint(Input.mousePosition).x, GameManager.GetInstance().cam.ScreenToWorldPoint(Input.mousePosition).y, hoverBox.transform.position.z);
        hoverBox.transform.position = new Vector3(GameManager.GetInstance().cam.ScreenToWorldPoint(Input.mousePosition).x, GameManager.GetInstance().cam.ScreenToWorldPoint(Input.mousePosition).y + 1);
        hoverBox.SetActive(true);
    }

    public void HideDescription()
    {
        hoverBox.SetActive(false);
    }
}
