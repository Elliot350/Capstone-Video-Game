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

    public void StartFight(List<Hero> heroes, List<Monster> monsters)
    {
        if (heroes.Count == 0 || monsters.Count == 0) 
            return;
        Debug.Log($"Starting fight between {heroes} and {monsters}");
        foreach (Monster monster in monsters)
        {
            monster.Die();
        }
    }
}
