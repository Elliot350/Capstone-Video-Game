using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockMonster : MonoBehaviour
{
    public Image image;
    public MonsterBase monsterBase;

    void Start()
    {
        image.sprite = monsterBase.GetSprite();
    }

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

    public void Clicked()
    {
        GameManager.GetInstance().monsterDescriptionBox.UnlockingMonster(monsterBase);
        UnlockManager.GetInstance().SelectedMonster(monsterBase);
        // UnlockManager.GetInstance().TryUnlock(monsterBase);
    }

    public void Hover()
    {
        Tooltip.ShowTooltip_Static(monsterBase.GetName() + (UnlockManager.GetInstance().IsMonsterUnlocked(monsterBase) ? " - (Unlocked)" : " - (Locked)"), 12);
    }

    public void EndHover()
    {
        Tooltip.HideTooltip_Static();
    }
}
