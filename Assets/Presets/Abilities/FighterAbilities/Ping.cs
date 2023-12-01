using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ping", menuName = "Abilities/Fighter/Ping")]
public class Ping : TriggeredFighterAbility
{
    [SerializeField] private float damage;
    [SerializeField] private int numOfTriggers;

    protected override void Activate(Fighter thisFighter)
    {
        // if (thisFighter.GetHealth() <= 0) return;
        List<Fighter> enemies = FightManager.GetInstance().GetEnemies(thisFighter);
        if (enemies.Count == 0) return;
        for (int i = 0; i < numOfTriggers; i++)
        {
            Damage attack = new Damage(thisFighter, enemies[Random.Range(0, enemies.Count)], damage);
            if (!animationTrigger.Equals("")) FightManager.GetInstance().AddAction(new PlayAnimation(attack.target, animationTrigger));
            FightManager.GetInstance().AddAction(new TakeDamage(attack));
        }
    }

    public override string GetDescription()
    {
        return string.Format(description, damage, numOfTriggers, triggers);
    }
}
