using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildTrapButton : MonoBehaviour
{
    public TrapBase trapBase;
    public Image image;

    // Start is called before the first frame update
    void Start()
    {
        image.sprite = trapBase.GetSprite();
    }

    public void HoverOver()
    {
        Tooltip.ShowTooltip_Static(trapBase);
    }

    public void HoverOut()
    {
        Tooltip.HideTooltip_Static();
    }

    public void Clicked()
    {
        // DungeonManager.GetInstance().BeginNewPlacement(trapBase);
    }
}
