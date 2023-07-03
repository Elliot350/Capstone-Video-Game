using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StartTurnPing", menuName = "Abilities/Fighter/Start Turn Ping")]
public class StartTurnPing : FighterAbility
{
    [SerializeField] private float damage;
    [SerializeField] private int triggers;

    public override void TurnStart(Fighter f)
    {
        List<Fighter> enemies =  FightManager.GetInstance().GetEnemies(f);

        for (int i = 0; i < triggers; i++)
        {
            FightManager.GetInstance().AddAction(new TakeDamage(new Damage(f, enemies[Random.Range(0, enemies.Count)], damage)));
        }
    }

    public override string GetDescription()
    {
        return string.Format(description, damage, triggers);
    }
}
