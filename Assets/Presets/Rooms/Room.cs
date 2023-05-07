using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomBase roomBase;

    public string displayName;
    public int monsterCapacity;
    public List<Monster> monsters;
    public List<Monster> currentMonsters;
    public int trapCapacity;
    public List<Trap> traps;
    public List<Trap> currentTraps;
    public GameObject highlightBox;

    public void SetType(RoomBase roomBase)
    {
        this.roomBase = roomBase;
        displayName = this.roomBase.GetName();
        monsterCapacity = this.roomBase.GetMonster();
        trapCapacity = this.roomBase.GetTrap();
        this.roomBase.AddRoom(this);
        roomBase.RoomBuilt(this);
    }

    public IEnumerator PartyEntered(Party party)
    {
        // Debug.Log($"PartyEntered");
        // If there are traps, wait a second and trigger them
        if (currentTraps.Count > 0)
        {
            yield return new WaitForSeconds(1);
            Debug.Log($"Triggering traps");
            foreach (Trap trap in currentTraps)
            {
                trap.PartyEntered(party);
            }
        }
        // If there are monsters, wait a second and fight them
        if (currentMonsters.Count > 0)
        {
            yield return new WaitForSeconds(1);
            Debug.Log($"Starting Fight");
            yield return FightManager.GetInstance().StartCoroutine(FightManager.GetInstance().StartFight(party.heroes, currentMonsters, this));
        }
    }

    public void AddMonster(MonsterBase monsterBase) 
    {
        Monster monster = Instantiate(MonsterPlacer.GetInstance().monsterPrefab, transform);
        monster.SetType(monsterBase);
        monsters.Add(monster);
        currentMonsters.Add(monster);
        roomBase.MonsterAdded(this, monster);
    }

    public void AddTrap(TrapBase trapBase)
    {
        Trap trap = Instantiate(TrapPlacer.GetInstance().trapPrefab, transform);
        trap.SetType(trapBase);
        traps.Add(trap);
        currentTraps.Add(trap);
        roomBase.TrapAdded(this, trap);
    }

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
        foreach (Trap trap in currentTraps)
        {
            trap.triggered = false;
        }
    }

    public void MonsterDied(Monster monster)
    {
        currentMonsters.Remove(monster);
    }

    public virtual void HeroesDefeatedMonsters()
    {
        roomBase.RoomDefeated(this);
    }


}
