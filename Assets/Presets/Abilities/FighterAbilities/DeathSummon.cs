using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeathSummon", menuName = "Abilities/Fighter/Death Summon")]
public class DeathSummon : FighterAbility
{
    [SerializeField] private MonsterBase monsterToSummon;
    [SerializeField] private int numberOfMonsters;

    public override void OnDeath(Damage attack)
    {
        for (int i = 0; i < numberOfMonsters; i++)
        {
            FightManager.GetInstance().AddAction(new Summon(attack.Target, monsterToSummon));
        }
    }

    public override string GetDescription()
    {
        return string.Format(description, monsterToSummon.GetName(), numberOfMonsters);
    }
}
