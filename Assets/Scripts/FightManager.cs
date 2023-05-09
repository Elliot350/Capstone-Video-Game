using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightManager : MonoBehaviour
{
    private static FightManager instance;

    public List<Fighter> fighters;

    private WaitForSeconds postAttackPause = new WaitForSeconds(1);

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

        fighters = new List<Fighter>();
        fighters.AddRange(monsters);
        fighters.AddRange(heroes);
        fighters.Sort((f1, f2)=>f1.GetSpeed().CompareTo(f2.GetSpeed()));

        // ShowFighters(fighters, room);
        foreach (Fighter f in fighters)
        {
            f.ShowFighter();
            Debug.Log($"Fighter: {f}");
        }
        
        int turn = 0;
        while (monsters.Count > 0 && heroes.Count > 0)
        {
            turn++;
            Debug.Log($"Turn: {turn}, Monsters: {monsters.Count}, Heroes: {heroes.Count}");
            // Debug.Log($"Before: Heroes: {heroes.Count}, Monsters: {monsters.Count}");
            for (int i = fighters.Count - 1; i >= 0; i--)
            {
                // Debug.Log($"{fighters[i]} attacking...");
                // Make sure the monster is still "alive"
                if (fighters[i] is Monster && monsters.Contains(fighters[i].GetComponent<Monster>()) && heroes.Count > 0)
                {
                    fighters[i].Attack(heroes);
                    // fighters[i].Die();
                    yield return postAttackPause;
                }
                else if (fighters[i] is Hero && heroes.Contains(fighters[i].GetComponent<Hero>()) && monsters.Count > 0)
                {
                    fighters[i].Attack(monsters);
                    yield return postAttackPause;
                }
                else
                {
                    Debug.LogWarning($"{fighters[i]} wasn't a hero or a monster!");
                    break;
                }
                // Debug.Log($"During: Heroes: {heroes.Count}, Monsters: {monsters.Count}");
            }
            // Debug.Log($"After: Heroes: {heroes.Count}, Monsters: {monsters.Count}");

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

    public void ShowFighters(List<Fighter> fighters, Room room)
    {
        float monsterOffset = 0f;
        float heroOffset = 0f;

        foreach (Fighter f in fighters)
        {
            if (f is Monster)
            {
                f.ShowFighter(room);
                Debug.Log($"Before: {f.transform.position}, {f.gameObject.transform.position}");
                f.gameObject.transform.position = new Vector3(f.transform.position.x - monsterOffset, f.transform.position.y, f.transform.position.z);
                monsterOffset += 1f;
                Debug.Log($"After: {f.transform.position}, {f.gameObject.transform.position}");
            }
            else if (f is Hero)
            {
                f.ShowFighter(room);
                f.gameObject.transform.position.Set(f.transform.position.x + heroOffset, f.transform.position.y, f.transform.position.z);
                heroOffset += 1f;
                Debug.Log($"{heroOffset}");
            }
            else
            {
                Debug.LogWarning($"{f} wasn't a hero or a monster!");
            }
        }
    }
}
