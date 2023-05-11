using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public RoomBase roomBase;

    public string displayName;
    public int monsterCapacity;
    public List<MonsterBase> monsters;
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
        if (monsters.Count > 0)
        {
            yield return new WaitForSeconds(1);
            Debug.Log($"Starting Fight");
            // TODO: Fix this line
            // yield return FightManager.GetInstance().StartCoroutine(FightManager.GetInstance().StartFight(party.heroes, monsters, this));
        }
    }

    public void AddMonster(MonsterBase monsterBase) 
    {
        monsters.Add(monsterBase);
        roomBase.MonsterAdded(this, monsterBase);
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
        foreach (Trap trap in currentTraps)
        {
            trap.triggered = false;
        }
    }

    public void MonsterDied(Monster monster)
    {
        roomBase.OnMonsterDied(monster);
    }

    public virtual void HeroesDefeatedMonsters()
    {
        roomBase.RoomDefeated(this);
    }


}
