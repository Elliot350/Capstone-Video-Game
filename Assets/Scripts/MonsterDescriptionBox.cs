using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterDescriptionBox : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI monsterName, monsterCost, monsterDescription, monsterTags, monsterHealth, monsterDamage;
    [SerializeField] private Image monsterImage;
    [SerializeField] private GameObject hoverBox;

    public void ShowDescription(MonsterBase monsterBase)
    {
        // Tooltip.ShowTooltip_Static(monsterBase.GetDescription(), 12);
        monsterName.text = monsterBase.GetName();
        monsterCost.text = monsterBase.GetCost().ToString();
        monsterDescription.text = monsterBase.GetDescription();
        monsterTags.text = Tag.FormatTags(monsterBase.GetTags());
        monsterHealth.text = monsterBase.GetMaxHealth().ToString();
        monsterDamage.text = monsterBase.GetDamage().ToString();
        monsterImage.sprite = monsterBase.GetSprite();
        
        hoverBox.SetActive(true);
    }

    public void HideDescription()
    {
        // Tooltip.HideTooltip_Static();
    }

    // public void UnlockingMonster(MonsterBase monsterBase)
    // {
    //     monsterName.text = monsterBase.GetName();
    //     monsterCost.text = monsterBase.GetCost().ToString();
    //     monsterDescription.text = monsterBase.GetDescription();
    //     monsterTags.text = Tag.FormatTags(monsterBase.GetTags());
    //     monsterHealth.text = monsterBase.GetMaxHealth().ToString();
    //     monsterDamage.text = monsterBase.GetDamage().ToString();

    //     hoverBox.SetActive(true);
    // }
}
