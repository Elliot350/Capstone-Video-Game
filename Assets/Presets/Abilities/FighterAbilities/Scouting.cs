using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Scouting", menuName = "Abilities/Fighter/Scouting")]
public class Scouting : TriggeredFighterAbility
{
    [SerializeField] private float healthBuff;
    [SerializeField] private float damageBuff;

    public override string GetDescription()
    {
        return string.Format(description, healthBuff, damageBuff);
    }

    protected override void Activate(Fighter self)
    {
        List<Fighter> allies = FightManager.GetInstance().GetAllies(self);
        Debug.Log($"Num of options: {allies.Count}");
        for (int i = 0; i < FightManager.GetInstance().GetEnemies(self).Count; i++)
        {
            int num = Random.Range(0, allies.Count);
            Debug.Log($"Selected: {num}");
            FightManager.GetInstance().AddAction(new BuffFighter(allies[num], healthBuff, damageBuff));
        }
    }
}
