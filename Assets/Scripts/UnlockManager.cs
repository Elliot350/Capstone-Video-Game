using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockManager : MonoBehaviour
{
    private static UnlockManager instance;

    public Color32 unlockedColor, unlockableColor, lockedColor;

    public GameObject monsterButton, monsterMenu;
    public List<GameObject> trees;
    
    public List<MonsterBase> unlockedMonsters;
    public MonsterDescriptionBox monsterDescriptionBox;
    public MonsterBase selectedMonster;

    public GameObject roomButton, roomMenu;
    public List<RoomBase> unlockedRooms;
    public RoomDescriptionBox roomDescriptionBox;
    public RoomBase selectedRoom;
    
    
    [Header("Temp stuff")]
    public Color unlockedLineColor;
    public Color lockedLineColor;
    

    private void Awake() 
    {
        instance = this;
        UpdateVisuals();
    }

    public static UnlockManager GetInstance()
    {
        return instance;
    }

    private void Unlock(MonsterBase monsterBase)
    {
        unlockedMonsters.Add(monsterBase);
        BuildMonsterButton button = Instantiate(monsterButton, monsterMenu.transform).GetComponent<BuildMonsterButton>();
        button.monsterBase = monsterBase;
        UpdateVisuals();
    }

    private void Unlock(RoomBase roomBase)
    {
        unlockedRooms.Add(roomBase);
        BuildRoomButton button = Instantiate(roomButton, roomMenu.transform).GetComponent<BuildRoomButton>();
        button.roomBase = roomBase;
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        foreach (GameObject tree in trees) 
        {
            foreach (Transform child in tree.transform)
            {
                // Some aren't UnlockMonsters need to change
                if (child.gameObject.TryGetComponent<UnlockMonster>(out UnlockMonster unlockMonster))
                    unlockMonster.UpdateVisuals();
                else if (child.gameObject.TryGetComponent<UnlockRoom>(out UnlockRoom unlockRoom))
                    unlockRoom.UpdateVisuals();
            }
        }
    }

    public void SelectedMonster(MonsterBase monsterBase)
    {
        selectedMonster = monsterBase;
        selectedRoom = null;
    }

    public void SelectedRoom(RoomBase roomBase)
    {
        selectedRoom = roomBase;
        selectedMonster = null;
    }

    public void TryUnlock(MonsterBase monsterBase)
    {
        if (monsterBase.IsUnlockable() && !unlockedMonsters.Contains(monsterBase))
        {
            Unlock(monsterBase);
        }
    }

    public void TryUnlock(RoomBase roomBase)
    {
        if (roomBase.IsUnlockable() && !unlockedRooms.Contains(roomBase))
        {
            Unlock(roomBase);
        }
    }

    public void TryUnlock()
    {
        if (selectedMonster != null) TryUnlock(selectedMonster);
        else if (selectedRoom != null) TryUnlock(selectedRoom);
    }

    public bool IsMonsterUnlocked(MonsterBase monsterBase)
    {
        return unlockedMonsters.Contains(monsterBase);
    }

    public bool IsRoomUnlocked(RoomBase roomBase)
    {
        return unlockedRooms.Contains(roomBase);
    }
}