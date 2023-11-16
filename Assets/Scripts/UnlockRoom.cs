using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockRoom : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private RoomBase roomBase;
    [SerializeField] private List<Line> lines = new List<Line>();

    void Start()
    {
        image.sprite = roomBase.GetSprite();
    }

    public void UpdateVisuals()
    {
        if (roomBase == null) return;

        if (UnlockManager.GetInstance().unlockedRooms.Contains(roomBase))
        {
            image.color = UnlockManager.GetInstance().unlockedColor;
            SetLineColours(UnlockManager.GetInstance().unlockedLineColor);
            return;
        }

        if (roomBase.IsUnlockable())
        {
            image.color = UnlockManager.GetInstance().unlockableColor;
            SetLineColours(UnlockManager.GetInstance().lockedLineColor);
            return;
        }

        SetLineColours(UnlockManager.GetInstance().lockedLineColor);
        image.color = UnlockManager.GetInstance().lockedColor;
    }

    private void SetLineColours(Color color)
    {
        foreach (Line l in lines)
        {
            l.SetColours(color);
        }
    }

    public void Clicked()
    {
        UnlockManager.GetInstance().roomDescriptionBox.ShowDescription(roomBase);
        UnlockManager.GetInstance().SelectedRoom(roomBase);
    }

    public void Hover()
    {
        Tooltip.ShowTooltip_Static(roomBase.GetName() + (UnlockManager.GetInstance().IsRoomUnlocked(roomBase) ? " - (Unlocked)" : " - (Locked)"), 12);
    }

    public void EndHover()
    {
        Tooltip.HideTooltip_Static();
    }
}
