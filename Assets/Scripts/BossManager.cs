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
    [SerializeField] private float totalHealthBuff, totalDamageBuff;
    [SerializeField] private List<FighterAbility> abilities;

    
    private FighterAbility chosenAbility, abilityOption1, abilityOption2;
    private BossUpgrade upgrade1, upgrade2;

    private void Awake()
    {
        Initialize();
    }
    
    public void Initialize()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Debug.LogWarning($"Duplicate BossManager");
            Destroy(gameObject);
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
            abilityOption1 = GameManager.GetInstance().GetRandomAbility((m) => !abilities.Contains(m));
            abilityOption2 = GameManager.GetInstance().GetRandomAbility((m) => !abilities.Contains(m) && m != abilityOption1);
            option1.Show(abilityUpgradeSprite, abilityTitle, abilityOption1.GetAbility());
            option2.Show(abilityUpgradeSprite, abilityTitle, abilityOption2.GetAbility());
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
                totalHealthBuff += 2f;
                break;
            case BossUpgrade.DAMAGE_UPGRADE:
                totalDamageBuff += 1f;
                break;
            case BossUpgrade.ABILITY_UPGRADE:
                abilities.Add(chosenAbility);
                break;
        }
        UIManager.GetInstance().CloseAllMenus();
    }

    public void Option1Picked()
    {
        chosenAbility = abilityOption1;
        OptionPicked(upgrade1);
    }

    public void Option2Picked()
    {
        chosenAbility = abilityOption2;
        OptionPicked(upgrade2);
    }
    
    public void ApplyBuffs(Fighter boss) 
    {
        boss.IncreaseDamage(totalDamageBuff);
        boss.IncreaseMaxHealth(totalHealthBuff);
        boss.GetAbilities().AddRange(abilities);
    }
}
