using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomBase room;

    public string displayName;
    public int monsterCapacity;
    public List<Monster> monsters;
    public List<Monster> currentMonsters;
    public int trapCapacity;
    public List<Trap> traps;
    public List<Trap> currentTraps;
    public GameObject highlightBox;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    public void SetType(RoomBase roomBase)
    {
        room = roomBase;
        room.SetType(this);
        room.AddRoom(this);
    }

    // TODO: Move this into RoomBase
    public void AddMonster(MonsterBase monsterBase) 
    {
        Monster monster = Instantiate(MonsterPlacer.GetInstance().monsterPrefab, transform);
        monster.SetType(monsterBase);
        monsters.Add(monster);
        currentMonsters.Add(monster);
    }

    // TODO: Move this into RoomBase
    public void AddTrap(TrapBase trapBase)
    {
        Trap trap = Instantiate(TrapPlacer.GetInstance().trapPrefab, transform);
        trap.SetType(trapBase);
        traps.Add(trap);
        currentTraps.Add(trap);
    }

    // TODO: Maybe move this into RoomBase
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
        currentMonsters.Remove(monster);
    }

    public virtual void HeroesDefeatedMonsters()
    {

    }

}
