using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Summoner", menuName = "Abilities/Fighter/Summoner")]
public class Summoner : FighterAbility
{
    [SerializeField] private MonsterBase monsterToSummon;

    public override void OnAttack(Damage attack)
    {
        FightManager.GetInstance().AddAction(new Summon(attack.Target, monsterToSummon));
    }

    public override string GetDescription()
    {
        return string.Format(description, monsterToSummon.GetName());
    }
}
