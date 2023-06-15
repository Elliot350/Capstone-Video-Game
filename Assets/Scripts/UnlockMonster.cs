using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockMonster : MonoBehaviour
{
    public Image image;
    public MonsterBase monsterBase;
    public List<Image> lines = new List<Image>();

    void Start()
    {
        image.sprite = monsterBase.GetSprite();
    }

    public void UpdateVisuals()
    {
        // If it is already unlocked
        if (UnlockManager.GetInstance().unlockedMonsters.Contains(monsterBase))
        {
            image.color = UnlockManager.GetInstance().unlockedColor;
            SetLine(UnlockManager.GetInstance().unlockedLineColor);
            return;
        }
        
        if (monsterBase.IsUnlockable())
        {
            image.color = UnlockManager.GetInstance().unlockableColor;
            SetLine(UnlockManager.GetInstance().lockedLineColor);
            return;
        }

        SetLine(UnlockManager.GetInstance().lockedLineColor);
        image.color = UnlockManager.GetInstance().lockedColor;
        
    }

    private void SetLine(Color color)
    {
        foreach (Image i in lines)
        {
            i.color = color;
        }
    }

    public void Clicked()
    {
        UnlockManager.GetInstance().monsterDescriptionBox.ShowDescription(monsterBase);
        UnlockManager.GetInstance().SelectedMonster(monsterBase);
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
