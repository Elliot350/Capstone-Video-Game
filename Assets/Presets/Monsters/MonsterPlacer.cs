using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MonsterPlacer : MonoBehaviour
{
    public static MonsterPlacer instance;

    private bool currentlyPlacing;
    private MonsterBase curMonsterBase;
    private float placementIndicatorUpdateRate = 0.05f;
    private float lastUpdateTime;
    private Vector3Int curPlacementPos;
    public GameObject placementIndicator;

    [SerializeField]
    public Monster monsterPrefab;

    public List<MonsterBase> monsterBases;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start() {
        monsterBases = Resources.LoadAll<MonsterBase>("").ToList();
    }

    public static MonsterPlacer GetInstance()
    {
        return instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CancelMonsterPlacement();
        if (Time.time - lastUpdateTime > placementIndicatorUpdateRate)
        {
            lastUpdateTime = Time.time;
            curPlacementPos = Selector.instance.GetCurTilePosition();
            placementIndicator.transform.position = curPlacementPos;
        }
        if (currentlyPlacing && Input.GetMouseButtonDown(0))
        {
            PlaceMonster();
        }
    }

    public void BeginNewMonsterPlacement(MonsterBase monsterBase) 
    {
        if (GameManager.GetInstance().money < monsterBase.cost)
            return;
        currentlyPlacing = true;
        curMonsterBase = monsterBase;
        placementIndicator.SetActive(true);
    }

    public void CancelMonsterPlacement()
    {
        currentlyPlacing = false;
        placementIndicator.SetActive(false);
        DungeonManager.GetInstance().HighlightRooms(false);
    }

    public void PlaceMonster()
    {
        PlaceMonster(curPlacementPos, curMonsterBase);
        CancelMonsterPlacement();
    }

    public void PlaceMonster(int x, int y, MonsterBase monsterBase)
    {
        PlaceMonster(new Vector3Int(x, y), monsterBase);
    }

    public void PlaceMonster(Vector3Int position, MonsterBase monsterBase)
    {
        foreach (Room room in DungeonManager.GetInstance().rooms)
        {
            if (room.transform.position.Equals(position)) {
                PlaceMonster(room, monsterBase);
                return;
            }
        }
    }

    public void PlaceMonster(Room room, MonsterBase monsterBase)
    {
        if (room.monsters.Count < room.monsterCapacity) {
            room.AddMonster(monsterBase);
            GameManager.GetInstance().SpendMoney(monsterBase.cost);
        }
    }
}  
