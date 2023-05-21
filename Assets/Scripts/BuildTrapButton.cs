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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HoverOver()
    {
        GameManager.GetInstance().trapDescriptionBox.ShowDesciption(trapBase);
    }

    public void HoverOut()
    {
        GameManager.GetInstance().trapDescriptionBox.HideDescription();
    }

    public void Clicked()
    {
        DungeonManager.GetInstance().BeginNewPlacement(trapBase);
    }
}
