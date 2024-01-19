using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterBuff", menuName = "Abilities/Room/Monster Buff")]
public class MonsterBuff : RoomAbility
{
    [SerializeField] private List<FighterBase> monsterBases;
    [SerializeField] private float damageBuff;
    [SerializeField] private float healthBuff;

    public override void CalculateStats(Room r, List<Fighter> monsters, List<Fighter> heroes)
    {
        foreach (Fighter f in monsters) {
            if (monsterBases.Contains(f.GetFighterType())) {
                f.IncreaseDamageModifier(damageBuff);
                f.IncreaseMaxHealthModifier(healthBuff);
            }
        }
    }

    public override string GetDescription()
    {
        return string.Format(description, damageBuff, healthBuff);
    }
}
