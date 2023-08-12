using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockMonster : MonoBehaviour
{
    public Image image;
    public MonsterBase monsterBase;
    public List<Line> lines = new List<Line>();

    void Start()
    {
        image.sprite = monsterBase.GetSprite();
    }

    public void UpdateVisuals()
    {
        if (monsterBase == null) return;
        // If it is already unlocked
        if (UnlockManager.GetInstance().unlockedMonsters.Contains(monsterBase))
        {
            image.color = UnlockManager.GetInstance().unlockedColor;
            SetLineColours(UnlockManager.GetInstance().unlockedLineColor);
            return;
        }
        
        if (monsterBase.IsUnlockable())
        {
            image.color = UnlockManager.GetInstance().unlockableColor;
            SetLineColours(UnlockManager.GetInstance().lockedLineColor);
            return;
        }

        SetLineColours(UnlockManager.GetInstance().lockedLineColor);
        image.color = UnlockManager.GetInstance().lockedColor;
        
    }

    private void SetLineColours(Color color)
    {
        foreach (Line l in lines)
        {
            l.SetColours(color);
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
