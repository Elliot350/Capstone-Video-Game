using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PullToFront", menuName = "Abilities/Fighter/Pull To Front")]
public class PullToFront : TriggeredFighterAbility
{
    private enum Target {
        RANDOM,
        INTERACTED,
        LOWEST_HEALTH,
        HIGHEST_HEALTH
    }

    [SerializeField] private Target target;

    protected override void Activate(Fighter self)
    {
        List<Fighter> enemies = FightManager.GetInstance().GetEnemies(self);
        switch (target)
        {
            case Target.RANDOM:
                if (enemies.Count == 0) return;
                Pull(self, enemies[Random.Range(0, enemies.Count)]);
                break;
            case Target.INTERACTED:
                break;
            case Target.LOWEST_HEALTH:
                if (enemies.Count == 0) return;
                int lowest = 0;
                // Find the enemy with the lowest health
                for (int i = 0; i < enemies.Count; i++) if (enemies[lowest].GetHealth() > enemies[i].GetHealth()) lowest = i;
                Pull(self, enemies[lowest]);
                break;
            case Target.HIGHEST_HEALTH:
                if (enemies.Count == 0) return;
                int highest = 0;
                // Find the enemy with the highest health
                for (int i = 0; i < enemies.Count; i++) if (enemies[highest].GetHealth() < enemies[i].GetHealth()) highest = i;
                Pull(self, enemies[highest]);
                break;   
            default:
                break;
        }
    }

    public override void OnAttack(Damage attack)
    {
        if (triggers.Contains(Trigger.ATTACK) && target == Target.INTERACTED)
            Pull(attack.Source, attack.Target);
        else if (triggers.Contains(Trigger.ATTACK))
            Activate(attack.Source);
    }

    public override void OnTakenDamage(Damage attack)
    {
        if (triggers.Contains(Trigger.DAMAGED) && target == Target.INTERACTED)
            Pull(attack.Target, attack.Source);
        else if (triggers.Contains(Trigger.DAMAGED))
            Activate(attack.Target);
    }

    public override void OnDeath(Damage attack)
    {
        if (triggers.Contains(Trigger.DEATH) && target == Target.INTERACTED)
            Pull(attack.Target, attack.Source);
        else if (triggers.Contains(Trigger.DEATH))
            Activate(attack.Target);
    }

    private void Pull(Fighter self, Fighter target)
    {
        if (target == null) return;
        FightManager.GetInstance().AddAction(new Move(target, 0));
    }
 
    public override string GetDescription()
    {
        return string.Format(description);
    }
}
