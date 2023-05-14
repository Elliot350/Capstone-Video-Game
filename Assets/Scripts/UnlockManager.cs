using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    private static UnlockManager instance;

    public GameObject monsterButton, monsterMenu;
    

    public List<MonsterBase> unlockedMonsters;

    private void Awake() 
    {
        instance = this;
    }

    public static UnlockManager GetInstance()
    {
        return instance;
    }

    private void Unlock(MonsterBase monsterBase)
    {
        unlockedMonsters.Add(monsterBase);
        BuildMonsterButton button = Instantiate(monsterButton, monsterMenu.transform).GetComponent<BuildMonsterButton>();
        button.monsterBase = monsterBase;
    }

    public void UpdateVisuals()
    {

    }

    public void TryUnlock(MonsterBase monsterBase)
    {
        if (monsterBase.IsUnlockable() && !unlockedMonsters.Contains(monsterBase))
        {
            Unlock(monsterBase);
        }
    }

    public bool MonsterUnlocked(MonsterBase monsterBase)
    {
        return unlockedMonsters.Contains(monsterBase);
    }

    
}