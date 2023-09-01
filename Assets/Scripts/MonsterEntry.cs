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
    private Room room;

    public void Set(MonsterBase monster, Room room)
    {
        this.monster = monster;
        this.room = room;
        image.sprite = monster.GetSprite();
        monsterName.text = monster.GetName();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        monster = null;
        room = null;
        gameObject.SetActive(false);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

    public void SellMonster()
    {
        room.SellMonster(monster);
        Hide();
    }
}
