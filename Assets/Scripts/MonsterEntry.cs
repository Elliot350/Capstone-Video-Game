using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MonsterEntry : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI monsterName;
    private MonsterBase monster;


    public void Set(MonsterBase monster)
    {
        this.monster = monster;
        image.sprite = monster.GetSprite();
        monsterName.text = monster.GetName();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        monster = null;
        gameObject.SetActive(false);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }
}
