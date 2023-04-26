using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI roomTitle, monstersText, trapsText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetRoom(Room room) 
    {
        // transform.position = GameManager.GetInstance().cam.WorldToViewportPoint(room.transform.position) - cam.transform.position;
        roomTitle.text = room.displayName.ToUpper();
        monstersText.text = "MONSTERS (" + room.monsters.Count + "/" + room.monsterCapacity +")";
        trapsText.text = "TRAPS (" + room.traps.Count + "/" + room.trapCapacity +")";
        gameObject.SetActive(true);
    }
}
