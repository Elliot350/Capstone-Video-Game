using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI roomTitle;
    [SerializeField] private GameObject monsterInfoPrefab, monsterInfoHolder;
    [SerializeField] private List<MonsterInfo> monsterInfos;
    
    private void Start() {
        gameObject.SetActive(false);
    }

    public void ShowRoom(Room room) 
    {
        // transform.position = GameManager.GetInstance().cam.WorldToViewportPoint(room.transform.position) - cam.transform.position;
        roomTitle.text = room.displayName.ToUpper();

        while (monsterInfos.Count < room.monsters.Count)
        {
            GameObject gameObject = Instantiate(monsterInfoPrefab, monsterInfoHolder.transform);
            MonsterInfo monsterInfo = gameObject.GetComponent<MonsterInfo>();
            monsterInfos.Add(monsterInfo);
        }

        foreach (MonsterInfo info in monsterInfos)
        {
            info.gameObject.SetActive(false);
        }

        for (int i = 0; i < room.monsters.Count; i++)
        {
            // monsterInfos[i].ShowMonster(room.monsters[i]);
        }

        gameObject.SetActive(true);
    }
}
