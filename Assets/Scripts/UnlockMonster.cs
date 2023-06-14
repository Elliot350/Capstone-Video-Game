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

    public void CreateLines()
    {
        foreach (MonsterBase mb in monsterBase.GetRequirements())
        {
            Vector3 buttonPosition = UnlockManager.GetInstance().GetPositionOfButton(mb);
            DrawLine(buttonPosition, transform.position, UnlockManager.GetInstance().lineColor, 10f);
        }
    }

    void DrawLine(Vector3 start, Vector3 end, Color color, float duration = 0.2f)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = UnlockManager.GetInstance().lineMaterial;
        lr.startColor = color;
        lr.startWidth = 10f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        GameObject.Destroy(myLine, duration);
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
