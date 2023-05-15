using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockManager : MonoBehaviour
{
    private static UnlockManager instance;
    public Color32 unlockedColor, unlockableColor, lockedColor;

    public GameObject monsterButton, monsterMenu, unlockTree;
    
    public List<MonsterBase> unlockedMonsters;

    private void Awake() 
    {
        instance = this;
        UpdateVisuals();
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
        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        foreach (Transform child in unlockTree.transform)
        {
            // Some aren't UnlockMonsters need to change
            child.gameObject.GetComponent<UnlockMonster>().UpdateVisuals();
        }
    }

    public void TryUnlock(MonsterBase monsterBase)
    {
        if (monsterBase.IsUnlockable() && !unlockedMonsters.Contains(monsterBase))
        {
            Unlock(monsterBase);
        }
    }

    public bool IsMonsterUnlocked(MonsterBase monsterBase)
    {
        return unlockedMonsters.Contains(monsterBase);
    }
}