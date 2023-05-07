using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrapDescriptionBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI trapName, trapCost, trapDescription;
    [SerializeField] private GameObject hoverBox;

    public void ShowDesciption(TrapBase trap)
    {
        trapName.text = trap.GetName();
        trapCost.text = trap.GetCost().ToString();
        trapDescription.text = trap.GetDescription();

        hoverBox.transform.position = new Vector3(GameManager.GetInstance().cam.ScreenToWorldPoint(Input.mousePosition).x, GameManager.GetInstance().cam.ScreenToWorldPoint(Input.mousePosition).y + 0.5f);
        hoverBox.SetActive(true);
    }

    public void HideDescription()
    {
        hoverBox.SetActive(false);
    }
}
