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
    private Room currentRoom;

    // [SerializeField] private TrapEntry trapEntryPrefab;
    // [SerializeField] private RectTransform trapContent;
    // [SerializeField] private List<TrapEntry> traps;

    public void DisplayRoom(Room room)
    {
        image.sprite = room.GetRoomBase().GetSprite();
        roomName.text = room.GetName();
        currentRoom = room;
        ShowMonsters(room);
        // ShowTraps(room);
    }

    public void Hide()
    {
        UIManager.GetInstance().CloseAllMenus();
    }

    private void ShowMonsters(Room room)
    {
        // Create new ones
        while (monsters.Count < room.GetMonsters().Count)
        {
            MonsterEntry newEntry = Instantiate(monsterEntryPrefab, monsterContent);
            monsters.Add(newEntry); 
            newEntry.SetRoomInfo(this);
        }
        // Delete extra ones
        while (monsters.Count > room.GetMonsters().Count)
        {
            monsters[0].Remove(); 
            monsters.RemoveAt(0);
        }

        // Show the entries
        for (int i = 0; i < room.GetMonsters().Count; i++)
        {
            monsters[i].Set(room.GetMonsters()[i], room);
        }

        // Size the content
        monsterContent.sizeDelta = new Vector2(monsterContent.sizeDelta.x, room.GetMonsters().Count * 75);
    }

    public void SellMonster(MonsterBase monsterBase, MonsterEntry entry)
    {
        currentRoom.SellMonster(monsterBase);
        monsters.Remove(entry);
        entry.Remove();
    }

    public void MoveEntryUp(MonsterEntry entry)
    {
        int index = monsters.IndexOf(entry);
        // If we are already the highest possible, return
        if (index == 0) return;
        
        // Remove and insert one spot earlier
        monsters.Remove(entry);
        monsters.Insert(index - 1, entry);
        entry.transform.SetSiblingIndex(index - 1);
    }

    public void MoveEntryDown(MonsterEntry entry)
    {
        int index = monsters.IndexOf(entry);
        // If we are already the lowest possible, return
        if (index == monsters.Count - 1) return;
        
        // Remove and insert one spot later
        monsters.Remove(entry);
        monsters.Insert(index + 1, entry);
        entry.transform.SetSiblingIndex(index + 1);
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
