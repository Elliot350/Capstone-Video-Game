using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    private static FightManager instance;

    private void Awake()
    {
        instance = this;
    }

    public static FightManager GetInstance()
    {
        return instance;
    }

    public IEnumerator StartFight(List<Hero> heroes, List<Monster> monsters, Room room)
    {
        if (heroes.Count == 0 || monsters.Count == 0) 
            yield break;
        
        // Temporary kill of all the monsters, will later replace with a fight
        for (int i = monsters.Count - 1; i >= 0; i--)
        {
            // yield return new WaitForSeconds(1);
            monsters[i].Die();
        }
        yield return new WaitForSeconds(1);

        if (heroes.Count == 0)
        {
            Debug.Log("The party has been defeated!");
            PartyManager.GetInstance().DestroyParty();
        }
        else
        {
            Debug.Log("The heroes defeated this room");
            room.HeroesDefeatedMonsters();
        }

    }
}
