using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrapEntry : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI trapName;
    private Trap trap;
    private Room room;

    public void Set(Trap trap, Room room)
    {
        this.trap = trap;
        this.room = room;
        gameObject.SetActive(true);
        image.sprite = trap.GetSprite();
        trapName.text = trap.displayName;
    }

    public void Hide()
    {
        trap = null;
        room = null;
        gameObject.SetActive(false);
    }

    public void Remove()
    {
        Destroy(gameObject);
    }

    public void SellMonster()
    {
        room.SellTrap(trap);
        Hide();
    }
}
