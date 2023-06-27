using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMonsterButton : MonoBehaviour
{
    public MonsterBase monsterBase;
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        image.sprite = monsterBase.GetSprite();
    }

    public void HoverOver()
    {
        Tooltip.ShowTooltip_Static(monsterBase);
    }

    public void HoverOut()
    {
        Tooltip.HideTooltip_Static();
    }

    public void Clicked()
    {
        DungeonManager.GetInstance().BeginNewPlacement(monsterBase);
    }
}
