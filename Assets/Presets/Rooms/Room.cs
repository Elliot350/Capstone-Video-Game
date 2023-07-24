using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Room : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    protected RoomBase roomType;

    protected string displayName;
    protected int monsterCapacity;
    [SerializeField] protected List<MonsterBase> monsters;
    protected int trapCapacity;
    [SerializeField] protected List<Trap> traps;
    protected List<RoomAbility> abilities;
    [SerializeField] protected GameObject highlightBox;
    [SerializeField] protected SpriteRenderer alertSprite;

    private double cooldown;
    private bool visited;

    private WaitForSeconds wait = new WaitForSeconds(1);

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
        this.roomType = roomBase;
        this.abilities = new List<RoomAbility>(roomBase.GetAbilities());
        displayName = this.roomType.GetName();
        monsterCapacity = this.roomType.GetMonster();
        trapCapacity = this.roomType.GetTrap();
        this.roomType.AddRoom(this);
        foreach (RoomAbility a in abilities)
            a.RoomBuilt(this);
    }

    public void TriggerPeriodic()
    {
        foreach (RoomAbility a in abilities)
            a.Periodic();
    }

    public void FighterDied(Fighter f)
    {
        foreach (RoomAbility a in abilities)
        {
            a.OnFighterDied(f);
        }
    }

    public void StartingFight(List<Fighter> monsters, List<Fighter> heroes)
    {
        foreach (RoomAbility a in abilities)
            a.FightStarted(monsters, heroes);
    }

    public void CalculateDamage(Fighter f)
    {
        foreach (RoomAbility a in abilities)
            a.CalculateDamage(f);
    }

    public void CalculateMaxHealth(Fighter f)
    {
        foreach (RoomAbility a in abilities)
            a.CalculateMaxHealth(f);
    }

    public virtual bool CanAddMonster(MonsterBase monster)
    {
        if (monsters.Count >= monsterCapacity)
            return false;
        foreach (RoomAbility a in abilities)
        {
            if (!a.CanAddMonster(this, monster))
                return false;
        }
        foreach (FighterAbility a in monster.GetAbilities())
        {
            if (!a.CanAddMonster(monster, this))
                return false;
        }
        return true;
    }

    public void AddMonster(MonsterBase monsterBase) 
    {
        monsters.Add(monsterBase);
        roomType.MonsterAdded(this, monsterBase);
        foreach (RoomAbility a in abilities)
            a.OnMonsterAdded(monsterBase);
    }

    public virtual bool CanAddTrap(TrapBase trap)
    {
        if (traps.Count >= trapCapacity)
            return false;
        foreach (RoomAbility a in abilities)
            if (!a.CanAddTrap(this, trap))
                return false;
        
        return true;
    }

    public void AddTrap(TrapBase trapBase)
    {
        // Trap trap = Instantiate(TrapPlacer.GetInstance().trapPrefab, transform);
        Trap trap = gameObject.AddComponent<Trap>();
        trap.SetType(trapBase);
        traps.Add(trap);
        roomType.TrapAdded(this, trap);
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

    public IEnumerator PartyEntered(Party party)
    {
        foreach (RoomAbility a in abilities)
            a.PartyEntered(party);

        if (TrapsUntriggered())
        {
            yield return wait;
            Debug.Log($"Triggering traps");
            foreach (Trap trap in traps)
            {
                trap.PartyEntered(party);
            }
        }
        // If there are monsters, wait a second and fight them
        if (monsters.Count > 0 && !visited)
        {
            yield return wait;
            Debug.Log($"Starting Fight");
            yield return FightManager.GetInstance().StartCoroutine(FightManager.GetInstance().StartFight(party.heroes, monsters, this));
            Debug.Log($"Fight Done?");
        }
        yield break;
    }


    public void Highlight(bool status) 
    {
        highlightBox.SetActive(status);
    }


    public virtual void ResetRoom()
    {
        foreach (Trap trap in traps)
        {
            trap.triggered = false;
        }
        visited = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.ShowTooltip_Static(GetStatus(), 12);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.HideTooltip_Static();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!DungeonManager.GetInstance().IsPlacing())
        {
            Debug.Log($"Room pressed");
            // GameManager.GetInstance().RoomClickedOn(this);
        }
    }

    public string GetStatus()
    {
        if (roomType is Hallway)
            return "Hallway";
        if (roomType is Entrance)
            return "Entrance";
        return $"{displayName}\nMonsters ({monsters.Count}/{monsterCapacity})\nTraps ({traps.Count}/{trapCapacity})";
    }



    public virtual void HeroesDefeatedMonsters()
    {
        roomType.RoomDefeated(this);
        foreach (RoomAbility a in abilities)
            a.PartyWon(PartyManager.GetInstance().GetParty());
        visited = true;
    }



    public string GetDescription() {return Ability.GetDescriptionFromList(abilities);}
    public List<RoomAbility> GetAbilities() {return abilities;}
    public List<MonsterBase> GetMonsters() {return monsters;}
    public int GetMonsterCapacity() {return monsterCapacity;}
    public List<Trap> GetTraps() {return traps;}
    public int GetTrapCapacity() {return trapCapacity;}
}
