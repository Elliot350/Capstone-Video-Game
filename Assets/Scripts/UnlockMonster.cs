using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockMonster : MonoBehaviour
{
    public Image image;
    public MonsterBase monsterBase;

    public void UpdateVisuals()
    {
        if (UnlockManager.GetInstance().unlockedMonsters.Contains(monsterBase))
        {
            image.color = UnlockManager.GetInstance().unlockedColor;
            return;
        }
        
        if (monsterBase.IsUnlockable())
        {
            image.color = UnlockManager.GetInstance().unlockableColor;
            return;
        }

        image.color = UnlockManager.GetInstance().lockedColor;
        
    }
}
