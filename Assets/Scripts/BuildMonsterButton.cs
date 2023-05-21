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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HoverOver()
    {
        GameManager.GetInstance().monsterDescriptionBox.ShowDescription(monsterBase);
    }

    public void HoverOut()
    {
        GameManager.GetInstance().monsterDescriptionBox.HideDescription();
    }

    public void Clicked()
    {
        DungeonManager.GetInstance().BeginNewPlacement(monsterBase);
    }
}
