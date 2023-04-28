using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public string displayName;
    public int monsterCapacity;
    public List<Monster> monsters = new List<Monster>();
    public List<Monster> currentMonsters;
    public int trapCapacity;
    public List<Trap> traps = new List<Trap>();
    public List<Trap> currentTraps;
    public GameObject highlightBox;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
        DungeonManager.GetInstance().rooms.Add(this);
        highlightBox.SetActive(false);
        currentMonsters = new List<Monster>();
        currentTraps = new List<Trap>();
    }

    public void SetType(RoomPreset roomPreset)
    {
        displayName = roomPreset.displayName;
        monsterCapacity = roomPreset.monsterCapacity;
        trapCapacity = roomPreset.trapCapacity;
    }

    public void AddMonster(MonsterPreset monsterPreset) 
    {
        Monster monster = Instantiate(MonsterPlacer.GetInstance().monsterPrefab, transform);
        monster.SetType(monsterPreset);
        monsters.Add(monster);
        currentMonsters.Add(monster);
    }

    public void PrintInfo() 
    {
        Debug.Log($"{displayName} is a room with a capacity of {monsterCapacity} (currently has {monsters})");
    }

    public void Highlight(bool status) 
    {
        highlightBox.SetActive(status);
    }

    public void SpawnMonsters() 
    {
        // foreach (GameObject monster in monsters)
        // {
        //     GameObject tmp = Instantiate(monster, transform);
        // }
    }

    public void PartyEntered(List<Hero> heroes) 
    {
        Debug.Log($"Party has entered a {displayName}");
        // Trigger any traps 
    }

    
    protected void OnMouseDown()
    {
        GameManager.GetInstance().RoomClickedOn(this);
    }

    public virtual void ResetRoom()
    {
        currentMonsters = new List<Monster>(monsters);
        // currentTraps = new List<Trap>(traps);
    }

    public void MonsterDied(Monster monster)
    {
        currentMonsters.Remove(monster);
    }
}
