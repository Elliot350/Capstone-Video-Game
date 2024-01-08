using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class Room : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    protected RoomBase roomType;

    protected string displayName;
    protected int monsterCapacity;
    [SerializeField] protected List<MonsterBase> monsters;
    // protected int trapCapacity;
    // [SerializeField] protected List<Trap> traps;
    protected List<RoomAbility> abilities;
    [SerializeField] protected GameObject highlightBox;
    [SerializeField] protected SpriteRenderer alertSprite;
    [SerializeField] protected List<Requirement> requirements;

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
        roomType = roomBase;
        abilities = new List<RoomAbility>();
        foreach (RoomAbility a in roomBase.GetAbilities())
            abilities.Add(Instantiate<RoomAbility>(a));
        displayName = roomType.GetName();
        monsterCapacity = roomType.GetMonster();
        // trapCapacity = roomType.GetTrap();
        roomType.AddRoom(this);
        foreach (RoomAbility a in abilities)
            a.RoomBuilt(this);
    }

    private void ActivateAbilities(Action<RoomAbility> action)
    {
        foreach (RoomAbility a in abilities)
            action(a);
    }

    public void TriggerPeriodic() {ActivateAbilities((a) => a.Periodic());}
    public void FighterDied(Fighter f) {ActivateAbilities((a) => a.OnFighterDied(this, f));}
    public void BattleStart(List<Fighter> monsters, List<Fighter> heroes) {ActivateAbilities((a) => a.BattleStart(this, monsters, heroes));}
    public void BattleEnd(List<Fighter> monsters, List<Fighter> heroes) {ActivateAbilities((a) => a.BattleEnd(this, monsters, heroes));}
    public void CalculateDamage(List<Fighter> monsters, List<Fighter> heroes) {ActivateAbilities((a) => a.CalculateDamage(this, monsters, heroes));}
    public void CalculateMaxHealth(List<Fighter> monsters, List<Fighter> heroes) {ActivateAbilities((a) => a.CalculateMaxHealth(this, monsters, heroes));}
    public void FighterAdded(Fighter f) {ActivateAbilities((a) => a.FighterSummoned(this, f));}
    public void FighterSummoned(Fighter fighter) {ActivateAbilities((a) => a.FighterSummoned(this, fighter));}

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
        // roomType.MonsterAdded(this, monsterBase);
        // foreach (RoomAbility a in abilities)
            // a.OnMonsterAdded(monsterBase);
    }

    // public virtual bool CanAddTrap(TrapBase trap)
    // {
    //     if (traps.Count >= trapCapacity)
    //         return false;
    //     foreach (RoomAbility a in abilities)
    //         if (!a.CanAddTrap(this, trap))
    //             return false;
        
    //     return true;
    // }

    // public void AddTrap(TrapBase trapBase)
    // {
    //     // Possibly need to change this
    //     // GameObject newGameObject = new GameObject("Trap");
    //     // newGameObject.transform.parent = this.gameObject.transform;
    //     Trap trap = gameObject.AddComponent<Trap>();
    //     trap.SetType(trapBase);
    //     traps.Add(trap);
    //     roomType.TrapAdded(this, trap);
    // }

    // public void TrapTriggered()
    // {
    //     alertSprite.gameObject.SetActive(true);
    //     cooldown = 1f;
    // }

    // private bool TrapsUntriggered()
    // {
    //     foreach (Trap trap in traps)
    //     {
    //         if (!trap.triggered)
    //             return true;
    //     }
    //     return false;
    // }

    public void Highlight(bool status) 
    {
        highlightBox.SetActive(status);
    }


    public virtual void ResetRoom()
    {
        // foreach (Trap trap in traps)
        // {
        //     trap.triggered = false;
        // }
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

            UIManager.GetInstance().ShowRoomInfo(this);
        }
    }

    public string GetStatus()
    {
        if (roomType is Hallway)
            return "Hallway";
        if (roomType is Entrance)
            return "Entrance";
        // return $"{displayName}\nMonsters ({monsters.Count}/{monsterCapacity})\nTraps ({traps.Count}/{trapCapacity})";
        return $"{displayName}\nMonsters ({monsters.Count}/{monsterCapacity})";
    }



    public virtual void HeroesDefeatedMonsters()
    {
        roomType.RoomDefeated(this);
        foreach (RoomAbility a in abilities)
            a.PartyWon(PartyManager.GetInstance().GetParty());
        visited = true;
    }

    public void SellMonster(MonsterBase monster)
    {
        Debug.Log($"Selling Monster {monster}");
        monsters.Remove(monster);
    }

    public void SellTrap(Trap trap)
    {

    }

    public void MoveMonster(MonsterBase monster, int newIndex)
    {
        monsters.Remove(monster);
        // if (newIndex >= monsters.Count) monsters.Add(monster);
        // else monsters.Insert(newIndex, monster);
        monsters.Insert(newIndex, monster);
    }

    public string GetDescription() {return Ability.GetDescriptionFromList(abilities);}
    public List<RoomAbility> GetAbilities() {return abilities;}
    public List<MonsterBase> GetMonsters() {return monsters;}
    public int GetMonsterCapacity() {return monsterCapacity;}
    // public List<Trap> GetTraps() {return traps;}
    // public int GetTrapCapacity() {return trapCapacity;}
    public RoomBase GetRoomBase() {return roomType;}
    public string GetName() {return displayName;}
    public bool BeenVisited() {return visited;}
}
