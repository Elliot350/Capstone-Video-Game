using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MonsterEntry : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI monsterName;
    private RoomInfo roomInfo;
    private MonsterBase monster;

    public void Set(MonsterBase monster, Room room)
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

    public void SellMonster()
    {
        roomInfo.SellMonster(monster, this);
    }

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        this.roomInfo = roomInfo;
    }

    public void MoveUp() 
    {
        Debug.Log($"Moving up");
        Debug.Log($"Old: {transform.GetSiblingIndex()}, After: {transform.GetSiblingIndex() - 1}");
        roomInfo.MoveEntryUp(this);
    }

    public void MoveDown() 
    {
        Debug.Log($"Moving down");
        Debug.Log($"Old: {transform.GetSiblingIndex()}, After: {transform.GetSiblingIndex() + 1}");
        roomInfo.MoveEntryDown(this);
    }
}
