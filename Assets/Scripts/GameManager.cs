using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    
    public int money;
    public int day;
    public TextMeshProUGUI statsText;

    public RoomInfo roomInfo;
    public Camera cam;

    public Animator roomMenu, trapMenu, monsterMenu;
    
    public GameObject errorPanel;
    public TextMeshProUGUI errorText;
    private bool error = false;
    private float errorTime;
    [SerializeField]
    private float errorDisplayTime;

    public Room tempRoom;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        FormatText();
    }

    // Update is called once per frame
    void Update()
    {
        if (error && errorTime + errorDisplayTime < Time.time) {
            errorPanel.SetActive(false);
            error = false;
        }

        if (Input.GetKeyDown(KeyCode.M)) {
            GainMoney(100);
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            DungeonManager.GetInstance().PlaceBasicDungeon();
        }

        if (Input.GetKeyDown(KeyCode.F)) {
            // FightManager.GetInstance().StartFight(PartyManager.GetInstance().party.heroes, tempRoom.monsters);
        }
    }


    public static GameManager GetInstance()
    {
        return instance;
    }

    public void OnPlaceBuilding(RoomPreset room)
    {
        
    }

    public void GainMoney(int amount)
    {
        money += amount;
        FormatText();
    }

    public void SpendMoney(int amount)
    {
        money -= amount;
        FormatText();
    }

    public void ErrorMessage(string message)
    {
        errorTime = Time.time;
        error = true;
        errorText.text = "[Error] " + message;
        errorPanel.SetActive(true);
    }

    public void FormatText()
    {
        statsText.text = $"${money}";
    }

    public void RoomClickedOn(Room room)
    {
        roomInfo.SetRoom(room);
    }

    public void CloseMenus() {
        Debug.Log($"Closing Menues");
        roomMenu.SetTrigger("Close");
        trapMenu.SetTrigger("Close");
        monsterMenu.SetTrigger("Close");
    }

    public void CloseMenus(Animator animator) {
        if (!animator.Equals(roomMenu)) {
            roomMenu.SetTrigger("Close");
        }
        if (!animator.Equals(trapMenu)) {
            trapMenu.SetTrigger("Close");
        }
        if (!animator.Equals(monsterMenu)) {
            monsterMenu.SetTrigger("Close");
        }
    }
}
