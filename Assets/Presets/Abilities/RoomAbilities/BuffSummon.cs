using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffSummon", menuName = "Abilities/Room/Buff Summon")]
public class BuffSummon : RoomAbility
{
    [SerializeField] private float healthBuff;
    [SerializeField] private float damageBuff;

    [SerializeField] private TargetType targetType;

    public override void OnFighterSummoned(Room r, Fighter f)
    {
        switch (targetType)
        {
            case TargetType.MONSTER: 
                if (f.isMonster) FightManager.GetInstance().AddAction(new BuffFighter(f, healthBuff, damageBuff)); 
                break;
            case TargetType.HERO:
                if (!f.isMonster) FightManager.GetInstance().AddAction(new BuffFighter(f, healthBuff, damageBuff)); 
                break;
            case TargetType.BOTH:
                FightManager.GetInstance().AddAction(new BuffFighter(f, healthBuff, damageBuff)); 
                break;
        }
    }

    public override string GetDescription()
    {
        return string.Format(description, damageBuff, healthBuff);
    }
}
