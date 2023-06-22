using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StartSummon", menuName = "Abilities/Fighter/Start Summon")]
public class StartSummon : FighterAbility
{
    [SerializeField] private MonsterBase summon;

    public override void OnBattleStarted(Fighter f)
    {
        FightManager.GetInstance().AddAction(new Summon(f, summon));
    }

    public override string GetDescription()
    {
        return string.Format(description, summon.GetName());
    }
}
