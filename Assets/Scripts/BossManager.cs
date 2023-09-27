using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    private static BossManager instance;

    public enum BossUpgrade {
        HEALTH_UPGRADE,
        DAMAGE_UPGRADE,
        ABILITY_UPGRADE
    }

    [SerializeField] private int bossLevel;
    [SerializeField] private string healthTitle, damageTitle, abilityTitle, healthText, damageText, abilityText;
    [SerializeField] private Sprite healthUpgradeSprite, damageUpgradeSprite, abilityUpgradeSprite;
    [SerializeField] private BossUpgradeOption option1, option2;


    private FighterAbility chosenAbility1, chosenAbility2;
    private BossUpgrade upgrade1, upgrade2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public static BossManager GetInstance() 
    {
        return instance;
    }

    public void LevelUp() 
    {
        bossLevel++;

        if (bossLevel % 5 == 0)
        {
            chosenAbility1 = GameManager.GetInstance().GetRandomAbility((m) => true);
            chosenAbility2 = GameManager.GetInstance().GetRandomAbility((m) => m != chosenAbility1);
            option1.Show(abilityUpgradeSprite, abilityTitle, chosenAbility1.GetAbility());
            option2.Show(abilityUpgradeSprite, abilityTitle, chosenAbility2.GetAbility());
            upgrade1 = BossUpgrade.ABILITY_UPGRADE;
            upgrade2 = BossUpgrade.ABILITY_UPGRADE;
        }
        else
        {
            option1.Show(healthUpgradeSprite, healthTitle, healthText);
            option2.Show(damageUpgradeSprite, damageTitle, damageText);
            upgrade1 = BossUpgrade.HEALTH_UPGRADE;
            upgrade2 = BossUpgrade.DAMAGE_UPGRADE;
        }

        UIManager.GetInstance().SetMenu(UIManager.MenuState.BOSS_UPGRADE_MENU);
    }

    

    public void OptionPicked(BossUpgrade upgrade) 
    {
        switch (upgrade)
        {
            case BossUpgrade.HEALTH_UPGRADE:
                break;
            case BossUpgrade.DAMAGE_UPGRADE:
                break;
            case BossUpgrade.ABILITY_UPGRADE:
                break;
        }
    }

    public void Option1Picked()
    {
        OptionPicked(upgrade1);
    }

    public void Option2Picked()
    {
        OptionPicked(upgrade2);
    }
    
}
