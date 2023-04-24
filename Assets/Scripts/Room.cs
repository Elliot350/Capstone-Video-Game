using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public string displayName;
    public List<Monster> monsters = new List<Monster>();
    public int monsterCapacity;
    public List<GameObject> traps = new List<GameObject>();
    public int trapCapacity;
    public GameObject highlightBox;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
        DungeonManager.GetInstance().rooms.Add(this);
        highlightBox.SetActive(false);

    }

    public void SetType(RoomPreset roomPreset)
    {
        displayName = roomPreset.displayName;
        monsterCapacity = roomPreset.monsterCapacity;
        trapCapacity = roomPreset.trapCapacity;
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

    
    void OnMouseDown()
    {
        Debug.Log($"{displayName} has been clicked");
    }

}
