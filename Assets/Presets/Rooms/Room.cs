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
    public List<RoomAbility> abilities;
    public GameObject highlightBox;
    public SpriteRenderer alertSprite;

    private double cooldown;
    private bool visited;

    private void Start() 
    {
        alertSprite.gameObject.SetActive(false);
    }

    void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0)
                alertSprite.gameObject.SetActive(false);
        }
    }

    public void SetType(RoomBase roomBase)
    {
        this.roomBase = roomBase;
        this.abilities = new List<RoomAbility>(roomBase.GetAbilities());
        displayName = this.roomBase.GetName();
        monsterCapacity = this.roomBase.GetMonster();
        trapCapacity = this.roomBase.GetTrap();
        this.roomBase.AddRoom(this);
        foreach (RoomAbility a in abilities)
            a.RoomBuilt(this);
    }

    public IEnumerator PartyEntered(Party party)
    {
        foreach (RoomAbility a in abilities)
            a.PartyEntered(party);

        if (TrapsUntriggered())
        {
            yield return new WaitForSeconds(1);
            Debug.Log($"Triggering traps");
            foreach (Trap trap in traps)
            {
                trap.PartyEntered(party);
            }
        }
        // If there are monsters, wait a second and fight them
        if (monsters.Count > 0 && !visited)
        {
            yield return new WaitForSeconds(1);
            Debug.Log($"Starting Fight");
            // TODO: Fix this line
            yield return FightManager.GetInstance().StartCoroutine(FightManager.GetInstance().StartFight(party.heroes, monsters, this));
            Debug.Log($"Fight Done?");
        }
    }

    public void StartingFight(List<Fighter> monsters, List<Fighter> heroes)
    {
        foreach (RoomAbility a in abilities)
            a.FightStarted(monsters, heroes);
    }

    public void AddMonster(MonsterBase monsterBase) 
    {
        monsters.Add(monsterBase);
        roomBase.MonsterAdded(this, monsterBase);
        foreach (RoomAbility a in abilities)
            a.OnMonsterAdded(monsterBase);
    }

    public void AddTrap(TrapBase trapBase)
    {
        // Trap trap = Instantiate(TrapPlacer.GetInstance().trapPrefab, transform);
        Trap trap = gameObject.AddComponent<Trap>();
        trap.SetType(trapBase);
        traps.Add(trap);
        roomBase.TrapAdded(this, trap);
    }

    public void TrapTriggered()
    {
        alertSprite.gameObject.SetActive(true);
        cooldown = 1f;
    }

    private bool TrapsUntriggered()
    {
        foreach (Trap trap in traps)
        {
            if (!trap.triggered)
                return true;
        }
        return false;
    }

    public float GetDamageMultiplier(Fighter f)
    {
        float multiplier = 0f;
        foreach (RoomAbility a in abilities)
            multiplier += a.GetDamageMultiplier(f);
        return multiplier;
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
        foreach (Trap trap in traps)
        {
            trap.triggered = false;
        }
        visited = false;
    }

    private void OnMouseEnter() 
    {
        Tooltip.ShowTooltip_Static(GetStatus(), 12);
    }

    private void OnMouseExit() 
    {
        Tooltip.HideTooltip_Static();
    }
    
    public string GetStatus()
    {
        if (roomBase is Hallway)
            return "Hallway";
        if (roomBase is Entrance)
            return "Entrance";
        return $"{displayName}\nMonsters ({monsters.Count}/{monsterCapacity})\nTraps ({traps.Count}/{trapCapacity})";
    }

    public string GetDescription()
    {
        return Ability.GetDescriptionFromList(abilities);
    }

    public void MonsterDied(Fighter f)
    {
        foreach (RoomAbility a in abilities)
            a.OnMonsterDied(f);
    }

    public void HeroDied(Fighter f)
    {
        foreach (RoomAbility a in abilities)
            a.OnHeroDied(f);
    }

    public virtual void HeroesDefeatedMonsters()
    {
        roomBase.RoomDefeated(this);
        foreach (RoomAbility a in abilities)
            a.PartyWon(PartyManager.GetInstance().GetParty());
        visited = true;
    }

    public virtual bool CanAddMonster(MonsterBase monster)
    {
        if (monsters.Count >= monsterCapacity)
            return false;
        foreach (RoomAbility a in abilities)
        {
            if (!a.CanAddMonster(monster))
                return false;
        }
        foreach (FighterAbility a in monster.GetAbilities())
        {
            if (!a.CanAddMonster(monster, this))
                return false;
        }
        return true;
    }

    public void TriggerPeriodic()
    {
        foreach (RoomAbility a in abilities)
            a.Periodic();
    }

    public List<RoomAbility> GetAbilities() {return abilities;}
}
