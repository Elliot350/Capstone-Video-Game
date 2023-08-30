using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfo : MonoBehaviour
{
    [SerializeField] private MonsterEntry monsterEntryPrefab;
    [SerializeField] private RectTransform content;
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private List<MonsterEntry> monsters;

    public void DisplayRoom(Room room)
    {
        image.sprite = room.GetRoomBase().GetSprite();
        roomName.text = room.GetName();
        
        ShowMonsters(room);
    }

    private void ShowMonsters(Room room)
    {
        foreach (MonsterEntry monsterEntry in monsters)
        {
            monsterEntry.Hide();
        }

        while (monsters.Count < room.GetMonsters().Count)
        {
            MonsterEntry newEntry = Instantiate(monsterEntryPrefab, content);
            monsters.Add(newEntry); 
        }

        for (int i = 0; i < room.GetMonsters().Count; i++)
        {
            monsters[i].Set(room.GetMonsters()[i]);
        }
        content.sizeDelta = new Vector2(content.sizeDelta.x, room.GetMonsters().Count * 50);
    }
}
