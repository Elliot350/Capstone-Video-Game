using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Summoning Boss", menuName = "Presets/Monsters/Bosses/Summoning Boss")]
public class SummoningBoss : BossBase
{
    public MonsterBase summoningMonster;

    public override void OnAttack()
    {
        FightManager.GetInstance().AddMonster(summoningMonster);
    }

}
