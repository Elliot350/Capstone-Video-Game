using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private Animator roomMenu, trapMenu, monsterMenu;
    [SerializeField] private GameObject roomMenuOpen, roomMenuClose, trapMenuOpen, trapMenuClose, monsterMenuOpen, monsterMenuClose;
    private const string OPEN = "Open", CLOSE = "Close";

    public void OpenRoomMenu()
    {
        CloseAllMenus();
        roomMenu.SetTrigger(OPEN);
        roomMenuOpen.SetActive(false);
        roomMenuClose.SetActive(true);
    }

    public void OpenTrapMenu()
    {
        CloseAllMenus();
        trapMenu.SetTrigger(OPEN);
        trapMenuOpen.SetActive(false);
        trapMenuClose.SetActive(true);
    }

    public void OpenMonsterMenu()
    {
        CloseAllMenus();
        monsterMenu.SetTrigger(OPEN);
        monsterMenuOpen.SetActive(false);
        monsterMenuClose.SetActive(true);
    }

    public void CloseAllMenus()
    {
        roomMenu.SetTrigger(CLOSE);
        trapMenu.SetTrigger(CLOSE);
        monsterMenu.SetTrigger(CLOSE);
        roomMenuOpen.SetActive(true);
        trapMenuOpen.SetActive(true);
        monsterMenuOpen.SetActive(true);
        roomMenuClose.SetActive(false);
        trapMenuClose.SetActive(false);
        monsterMenuClose.SetActive(false);
    }
}
