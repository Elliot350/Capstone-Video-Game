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

    public IEnumerator PartyEntered(Party party)
    {
        Debug.Log($"PartyEntered");
        // If there are traps, wait a second and trigger them
        if (currentTraps.Count > 0)
        {
            yield return new WaitForSeconds(1);
            Debug.Log($"Triggering traps");
            room.PartyEntered(this, party);
        }
        // If there are monsters, wait a second and fight them
        if (currentMonsters.Count > 0)
        {
            yield return new WaitForSeconds(1);
            Debug.Log($"Starting Fight");
            yield return FightManager.GetInstance().StartCoroutine(FightManager.GetInstance().StartFight(party.heroes, currentMonsters, this));
        }
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
        room.RoomDefeated(this);
    }


}
