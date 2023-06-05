using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChooser : MonoBehaviour
{
    [SerializeField] private MonsterDescriptionBox option1, option2;
    [SerializeField] private MonsterBase boss1, boss2;

    // Start is called before the first frame update
    void Start()
    {
        option1.ShowDescription(boss1);
        option2.ShowDescription(boss2);
    }

    public void Boss1Picked()
    {
        GameManager.GetInstance().AddBoss(boss1);
        UIManager.GetInstance().SetMenu(UIManager.MenuState.GAME);
    }

    public void Boss2Picked()
    {
        GameManager.GetInstance().AddBoss(boss2);
        UIManager.GetInstance().SetMenu(UIManager.MenuState.GAME);
    }

    
}
