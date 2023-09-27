using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomName;
    [SerializeField] private Image image;
    [SerializeField] private MonsterEntry monsterEntryPrefab;
    [SerializeField] private RectTransform monsterContent;
    [SerializeField] private List<MonsterEntry> monsters;

    [SerializeField] private TrapEntry trapEntryPrefab;
    [SerializeField] private RectTransform trapContent;
    [SerializeField] private List<TrapEntry> traps;

    public void DisplayRoom(Room room)
    {
        image.sprite = room.GetRoomBase().GetSprite();
        roomName.text = room.GetName();
        
        ShowMonsters(room);
        // ShowTraps(room);
    }

    private void ShowMonsters(Room room)
    {
        // Hide all of them just in case
        foreach (MonsterEntry monsterEntry in monsters)
        {
            monsterEntry.Hide();
        }

        // Create the new ones
        while (monsters.Count < room.GetMonsters().Count)
        {
            MonsterEntry newEntry = Instantiate(monsterEntryPrefab, monsterContent);
            monsters.Add(newEntry); 
        }

        // Show the entries
        for (int i = 0; i < room.GetMonsters().Count; i++)
        {
            monsters[i].Set(room.GetMonsters()[i], room);
        }

        // Size the content
        monsterContent.sizeDelta = new Vector2(monsterContent.sizeDelta.x, room.GetMonsters().Count * 50);
    }

    // private void ShowTraps(Room room)
    // {
    //     // Hide all of them just in case
    //     foreach (TrapEntry trapEntry in traps)
    //     {
    //         trapEntry.Hide();
    //     }

    //     // Create the new ones
    //     while (traps.Count < room.GetTraps().Count)
    //     {
    //         TrapEntry newEntry = Instantiate(trapEntryPrefab, trapContent);
    //         traps.Add(newEntry);
    //     }

    //     // Show the entries
    //     for (int i = 0; i < room.GetTraps().Count; i++)
    //     {
    //         traps[i].Set(room.GetTraps()[i], room);
    //     }

    //     // Size the content
    //     trapContent.sizeDelta = new Vector2(monsterContent.sizeDelta.x, room.GetTraps().Count * 50);
    // }
}
