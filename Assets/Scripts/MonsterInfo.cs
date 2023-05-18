using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI monsterName, healthText, damageText, costText, descriptionText, tagsText;
    [SerializeField] private Image monsterImage;
    
    private void Start()
    {
        
    }

    public void ShowMonster(MonsterBase mb)
    {
        monsterName.text = mb.GetName();
        healthText.text = mb.GetMaxHealth().ToString();
        damageText.text = mb.GetDamage().ToString();
        costText.text = mb.GetCost().ToString();
        descriptionText.text = mb.GetDescription();
        monsterImage.sprite = mb.GetSprite();
        tagsText.text = Tag.FormatTags(mb.GetTags());
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
