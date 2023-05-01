using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MonsterPlacer : MonoBehaviour
{
    public static MonsterPlacer instance;

    private bool currentlyPlacing;
    private MonsterPreset curMonsterPreset;
    private float placementIndicatorUpdateRate = 0.05f;
    private float lastUpdateTime;
    private Vector3Int curPlacementPos;
    public GameObject placementIndicator;

    [SerializeField]
    public Monster monsterPrefab;

    public List<MonsterPreset> monsterPresets;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start() {
        monsterPresets = Resources.LoadAll<MonsterPreset>("").ToList();
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

    public void BeginNewMonsterPlacement(MonsterPreset monsterPreset) 
    {
        if (GameManager.GetInstance().money < monsterPreset.cost)
            return;
        currentlyPlacing = true;
        curMonsterPreset = monsterPreset;
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
        PlaceMonster(curPlacementPos, curMonsterPreset);
        CancelMonsterPlacement();
    }

    public void PlaceMonster(int x, int y, MonsterPreset monsterPreset)
    {
        PlaceMonster(new Vector3Int(x, y), monsterPreset);
    }

    public void PlaceMonster(Vector3Int position, MonsterPreset monsterPreset)
    {
        foreach (Room room in DungeonManager.GetInstance().rooms)
        {
            if (room.transform.position.Equals(position)) {
                PlaceMonster(room, monsterPreset);
                return;
            }
        }
    }

    public void PlaceMonster(Room room, MonsterPreset monsterPreset)
    {
        if (room.monsters.Count < room.monsterCapacity) {
            room.AddMonster(monsterPreset);
            GameManager.GetInstance().SpendMoney(monsterPreset.cost);
        }
    }
}  
