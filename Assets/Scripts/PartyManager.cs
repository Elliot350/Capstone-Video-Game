using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PartyManager : MonoBehaviour
{
    private static PartyManager instance;

    public Party party;

    public Party partyPrefab;
    public Hero heroPrefab;

    [SerializeField]
    private List<HeroPreset> heroPresets;

    public float moveTime;
    private float lastMoveTime;

    private void Awake()
    {
        instance = this;
    }

    public static PartyManager GetInstance()
    {
        return instance;
    }
    
    private void Start()
    {
        heroPresets = Resources.LoadAll<HeroPreset>("").ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (party != null && Time.time - lastMoveTime > moveTime) {
            Move();
        }   
    }

    public void CreateHero(HeroPreset heroPreset)
    {
        Hero heroTemp = Instantiate(heroPrefab, party.transform);
        heroTemp.SetType(heroPreset);
        party.AddHero(heroTemp);
    }

    // Create a random party
    public void CreateParty()
    {
        CreateParty(new List<HeroPreset>{heroPresets[0]});
    }

    public void CreateParty(List<HeroPreset> list)
    {
        party = Instantiate(partyPrefab, Vector3.zero, Quaternion.identity);
        foreach (HeroPreset hero in list)
        {
            CreateHero(hero);
        }
    }

    private void Move() {
        Vector3Int nextPosition = Vector3Int.FloorToInt(party.transform.position);
        nextPosition.x += 1;
        if (RoomPlacer.GetInstance().tilemap.GetTile(nextPosition) != null) {
            lastMoveTime = Time.time;
            party.transform.position = nextPosition;
            JustMoved();
            return;
        }

        nextPosition = Vector3Int.FloorToInt(party.transform.position);
        nextPosition.y += 1;
        if (RoomPlacer.GetInstance().tilemap.GetTile(nextPosition) != null) {
            lastMoveTime = Time.time;
            party.transform.position = nextPosition;
            JustMoved();
            return;
        }
    }

    private void JustMoved() {
        GameObject roomGameObject = RoomPlacer.GetInstance().tilemap.GetInstantiatedObject(Vector3Int.FloorToInt(party.transform.position));
        Room room = roomGameObject.GetComponent<Room>();
        room.PartyEntered(party.heroes);
        if (room != null) {
            FightManager.GetInstance().StartFight(party.heroes, room.monsters);
        }
    }
}
