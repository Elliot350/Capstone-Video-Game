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
        // for (int i = monsters.Count - 1; i >= 0; i--)
        // {
        //     // yield return new WaitForSeconds(1);
        //     monsters[i].Die();
        // }

        List<Fighter> fighters = new List<Fighter>();
        fighters.AddRange(monsters);
        fighters.AddRange(heroes);
        
        int count = 0;

        while (monsters.Count > 0 && heroes.Count > 0)
        {
            count++;
            Debug.Log($"Heroes: {heroes.Count}, Monsters: {monsters.Count}");
            foreach (Fighter f in fighters)
            {
                if (f is Monster && heroes.Count > 0)
                {
                    f.Attack(heroes[0]);
                    f.Die();
                }
                else if (f is Hero && monsters.Count > 0)
                {
                    f.Attack(monsters[0]);
                }
                else
                {
                    break;
                }
            }

            if (count >= 10)
            {
                break;
            }
        }

        yield return new WaitForSeconds(1);
        Debug.Log("Fight Resolved");

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
