using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public enum MenuState {
        GAME,
        FIGHT,
        PICK_BOSS,
        UNLOCK_MONSTER,
        UNLOCK_TRAP,
        UNLOCK_ROOM,
    }
    MenuState state = MenuState.GAME;

    [Header("Fight Menu")]
    [SerializeField] private GameObject fightMenu;

    [Header("Pick Boss")]
    [SerializeField] private GameObject bossMenu;

    [Header("Unlock Menu")]
    [SerializeField] private GameObject unlockMenu;

    [Header("Build Menu")]
    [SerializeField] private Animator roomMenu, trapMenu, monsterMenu;
    [SerializeField] private GameObject roomMenuOpen, roomMenuClose, trapMenuOpen, trapMenuClose, monsterMenuOpen, monsterMenuClose;
    private const string OPEN = "Open", CLOSE = "Close";
    private bool roomOpen, trapOpen, monsterOpen;

    private void Awake()
    {
        instance = this;
    }

    public static UIManager GetInstance()
    {
        return instance;
    }

    // ---------- General Menu Stuff ----------

    private void SetMenus()
    {
        switch (state)
        {
            case MenuState.GAME:
                CloseAllMenus();
                break;
            case MenuState.FIGHT:
                CloseAllMenus();
                SetFightMenu(true);
                break;
            case MenuState.PICK_BOSS:
                CloseAllMenus();
                SetBossMenu(true);
                break;
            case MenuState.UNLOCK_MONSTER:
                CloseAllMenus();
                SetUnlockMenu(true);
                break;
            default:
                break;
        }
    }

    public void SetMenu(MenuState menuState)
    {
        state = menuState;
        SetMenus();
    }

    public void CloseAllMenus()
    {
        SetFightMenu(false);
        SetUnlockMenu(false);
        SetBossMenu(false);
    }

    // ---------- Fight Menu ----------

    private void SetFightMenu(bool active)
    {
        fightMenu.SetActive(active);
    }

    public void OpenFightMenu()
    {
        SetMenu(MenuState.FIGHT);
    }

    // ---------- Boss Menu ---------

    private void SetBossMenu(bool active)
    {
        bossMenu.SetActive(active);
    }

    public void OpenBossMenu()
    {
        SetMenu(MenuState.PICK_BOSS);
    }

    // ---------- Unlock Menu ----------

    private void SetUnlockMenu(bool active)
    {
        unlockMenu.SetActive(active);
    }

    public void OpenUnlockMenu()
    {
        SetMenu(MenuState.UNLOCK_MONSTER);
    }

    // ---------- Build Menus ----------

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

    public void CloseBuildMenus()
    {
        CloseRoomMenu();
        CloseTrapMenu();
        CloseMonsterMenu();
    }
}
