using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Animator roomMenu, trapMenu, monsterMenu;
    [SerializeField] private GameObject roomMenuOpen, roomMenuClose, trapMenuOpen, trapMenuClose, monsterMenuOpen, monsterMenuClose;
    private const string OPEN = "Open", CLOSE = "Close";
    private bool roomOpen, trapOpen, monsterOpen;

    public void OpenRoomMenu()
    {
        CloseTrapMenu();
        CloseMonsterMenu();
        roomOpen = true;
        roomMenu.SetTrigger(OPEN);
        roomMenuOpen.SetActive(false);
        roomMenuClose.SetActive(true);
    }

    public void OpenTrapMenu()
    {
        CloseRoomMenu();
        CloseMonsterMenu();
        trapOpen = true;
        trapMenu.SetTrigger(OPEN);
        trapMenuOpen.SetActive(false);
        trapMenuClose.SetActive(true);
    }

    public void OpenMonsterMenu()
    {
        CloseRoomMenu();
        CloseTrapMenu();
        monsterOpen = true;
        monsterMenu.SetTrigger(OPEN);
        monsterMenuOpen.SetActive(false);
        monsterMenuClose.SetActive(true);
    }

    public void CloseRoomMenu()
    {
        if (!roomOpen)
            return;
        roomOpen = false;
        roomMenu.SetTrigger(CLOSE);
        roomMenuOpen.SetActive(true);
        roomMenuClose.SetActive(false);
    }

    public void CloseTrapMenu()
    {
        if (!trapOpen)
            return;
        trapOpen = false;
        trapMenu.SetTrigger(CLOSE);
        trapMenuOpen.SetActive(true);
        trapMenuClose.SetActive(false);
    }

    public void CloseMonsterMenu()
    {
        if (!monsterOpen)
            return;
        monsterOpen = false;
        monsterMenu.SetTrigger(CLOSE);
        monsterMenuOpen.SetActive(true);
        monsterMenuClose.SetActive(false);
    }

    public void CloseAllMenus()
    {
        CloseRoomMenu();
        CloseTrapMenu();
        CloseMonsterMenu();
    }
}
