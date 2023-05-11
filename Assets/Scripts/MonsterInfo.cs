using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI monsterName, healthText, damageText;
    [SerializeField] private Image monsterImage;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowMonster(Monster monster)
    {
        monsterName.text = monster.GetName().ToUpper();
        healthText.text = monster.GetMaxHealth().ToString();
        damageText.text = monster.GetDamage().ToString();
        monsterImage.sprite = monster.GetSprite();
        gameObject.SetActive(true);
    }
}
