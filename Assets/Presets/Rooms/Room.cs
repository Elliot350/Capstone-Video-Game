using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomPreset room;

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
        room = roomPreset;
        displayName = room.displayName;
        monsterCapacity = room.monsterCapacity;
        trapCapacity = room.trapCapacity;
    }

    public void AddMonster(MonsterPreset monsterPreset) 
    {
        Monster monster = Instantiate(MonsterPlacer.GetInstance().monsterPrefab, transform);
        monster.SetType(monsterPreset);
        monsters.Add(monster);
        currentMonsters.Add(monster);
    }

    public void AddTrap(TrapPreset trapPreset)
    {
        Trap trap = Instantiate(TrapPlacer.GetInstance().trapPrefab, transform);
        trap.SetType(trapPreset);
        traps.Add(trap);
        currentTraps.Add(trap);
    }

    public void Highlight(bool status) 
    {
        highlightBox.SetActive(status);
    }

    public void PartyEntered(Party party) 
    {
        // Trigger any traps that tigger when the party enters
        room.PartyEntered(this, party);
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
        // currentMonsters.Remove(monster);
    }

    public virtual void HeroesDefeatedMonsters()
    {

    }

}
