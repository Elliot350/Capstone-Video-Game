using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    public enum MenuState {
        GAME,
        PAUSE,
        FIGHT,
        PICK_BOSS,
        ROOM_INFO,
        TUTORIAL,
        BOSS_UPGRADE_MENU,
        UNLOCK_MONSTER,
        UNLOCK_ROOM
    }
    MenuState state = MenuState.GAME;

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenu;

    [Header("Fight Menu")]
    [SerializeField] private GameObject fightMenu;

    [Header("Pick Boss")]
    [SerializeField] private GameObject bossMenu;

    [Header("Unlock Menu")]
    [SerializeField] private GameObject unlockMenu;

    [Header("Room Info Menu")]
    [SerializeField] private GameObject roomInfoMenu;

    [Header("Tutorial Menu")]
    [SerializeField] private TutorialManager tutorialManager;
    
    [Header("Boss Upgrade Menu")]
    [SerializeField] private GameObject bossUpgradeMenu;

    private Dictionary<MenuState, GameObject> menus;


    [Header("Build Menu")]
    [SerializeField] private Animator roomMenu;
    // [SerializeField] private Animator trapMenu;
    [SerializeField] private Animator monsterMenu;
    [SerializeField] private GameObject roomMenuOpen, roomMenuClose, /*trapMenuOpen, trapMenuClose,*/ monsterMenuOpen, monsterMenuClose;
    private const string OPEN = "Open", CLOSE = "Close";
    private bool roomOpen, /*trapOpen,*/ monsterOpen;

    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private HealthBar healthBar;
    
    public void Initialize()
    {
        Debug.Log($"UIManager initializing");
        if (instance == null)
        {
            instance = this;
            if (!PlayerPrefsManager.hasSeenTutorial)
            {
                SetMenu(MenuState.TUTORIAL);
                tutorialManager.ShowStep(0);
            }
            menus = new Dictionary<MenuState, GameObject>{
                {MenuState.PAUSE, pauseMenu},
                {MenuState.FIGHT, fightMenu},
                {MenuState.PICK_BOSS, bossMenu},
                {MenuState.ROOM_INFO, roomInfoMenu},
                {MenuState.TUTORIAL, tutorialManager.gameObject},
                {MenuState.BOSS_UPGRADE_MENU, bossUpgradeMenu},
                {MenuState.UNLOCK_MONSTER, unlockMenu}
            };
        }
        else 
        {
            Debug.LogWarning($"Duplicate UIManager");
            Destroy(gameObject);
        }
    }

    public static UIManager GetInstance()
    {
        return instance;
    }

    // ---------- General Menu Stuff ----------

    public void SetMenu(MenuState menuState)
    {
        // Close the one currently open
        // Try to close automatically
        SetMenu(state, false);
        switch (state)
        {
            case MenuState.GAME:
                break;
            case MenuState.PAUSE:
                pauseMenu.SetActive(false);
                Time.timeScale = 1f;
                break;
            case MenuState.TUTORIAL:
                tutorialManager.Hide();
                break;
            default:
                break;
        }
        // Then open the next one
        state = menuState;
        // Try to open automatically
        SetMenu(state, true);
        switch (state)
        {
            case MenuState.GAME:
                break;
            case MenuState.PAUSE:
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
                break;
            case MenuState.TUTORIAL:
                tutorialManager.Show();
                break;
            default:
                break;
        }
        // SetMenus();
    }

    public void CloseAllMenus()
    {
        SetMenu(MenuState.GAME);
        foreach (MenuState key in menus.Keys)
        {
            menus[key].SetActive(false);
        }
    }

    public void OpenPauseMenu()
    {
        SetMenu(MenuState.PAUSE);
    }

    public void SetMenu(MenuState menu, bool status)
    {
        GameObject menuToSet;
        if (menus.TryGetValue(menu, out menuToSet))
        {
            menuToSet.SetActive(status);
        }
    }

    // ---------- Fight Menu ----------

    public void OpenFightMenu()
    {
        SetMenu(MenuState.FIGHT);
    }

    // ---------- Boss Menu ---------

    public void OpenBossMenu()
    {
        SetMenu(MenuState.PICK_BOSS);
    }

    // ---------- Unlock Menu ----------

    public void OpenUnlockMenu()
    {
        SetMenu(MenuState.UNLOCK_MONSTER);
    }

    // ---------- Room Info Menu ----------

    public void ShowRoomInfo(Room room)
    {
        roomInfoMenu.GetComponent<RoomInfo>().DisplayRoom(room);
        SetMenu(MenuState.ROOM_INFO);
    }

    // ---------- Build Menus ----------

    public void OpenRoomMenu()
    {
        // CloseTrapMenu();
        CloseMonsterMenu();
        roomOpen = true;
        roomMenu.SetTrigger(OPEN);
        roomMenuOpen.SetActive(false);
        roomMenuClose.SetActive(true);
    }

    // public void OpenTrapMenu()
    // {
    //     CloseRoomMenu();
    //     CloseMonsterMenu();
    //     trapOpen = true;
    //     trapMenu.SetTrigger(OPEN);
    //     trapMenuOpen.SetActive(false);
    //     trapMenuClose.SetActive(true);
    // }

    public void OpenMonsterMenu()
    {
        CloseRoomMenu();
        // CloseTrapMenu();
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

    // public void CloseTrapMenu()
    // {
    //     if (!trapOpen)
    //         return;
    //     trapOpen = false;
    //     trapMenu.SetTrigger(CLOSE);
    //     trapMenuOpen.SetActive(true);
    //     trapMenuClose.SetActive(false);
    // }

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
        // CloseTrapMenu();
        CloseMonsterMenu();
    }

    // ---------- Text Stuff ----------

    public void SetText()
    {
        moneyText.text = $"${GameManager.GetInstance().GetMoney().ToString()}";
        manaText.text = $"{GameManager.GetInstance().GetMana().ToString()}M";
        healthBar.Set(GameManager.GetInstance().GetHealth(), GameManager.GetInstance().GetMaxHealth());
    }
}
