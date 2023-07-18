using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ping", menuName = "Abilities/Fighter/Ping")]
public class Ping : FighterAbility
{
    [SerializeField] private float damage;
    [SerializeField] private int numOfTriggers;
    // Maybe make this a list so there can be multiple triggers?
    [SerializeField] private Trigger trigger;

    private void Activate(Fighter thisFighter)
    {
        List<Fighter> enemies = FightManager.GetInstance().GetEnemies(thisFighter);
        if (enemies.Count == 0) return;
        for (int i = 0; i < numOfTriggers; i++)
        {
            Damage attack = new Damage(thisFighter, enemies[Random.Range(0, enemies.Count)], damage);
            if (!animationTrigger.Equals("")) FightManager.GetInstance().AddAction(new PlayAnimation(attack.Target, animationTrigger));
            FightManager.GetInstance().AddAction(new TakeDamage(attack));
        }
    }

    public override void BattleStart(Fighter f) {if (trigger == Trigger.START_BATTLE) Activate(f);}
    public override void BattleEnd(Fighter f) {if (trigger == Trigger.END_BATTLE) Activate(f);}
    public override void TurnStart(Fighter f) {if (trigger == Trigger.START_TURN) Activate(f);}
    public override void TurnEnd(Fighter f) {if (trigger == Trigger.END_TURN) Activate(f);}
    public override void OnAttack(Damage attack) {if (trigger == Trigger.ATTACK) Activate(attack.Source);}
    public override void OnTakenDamage(Damage attack) {if (trigger == Trigger.DAMAGED) Activate(attack.Target);}
    public override void OnDeath(Damage attack) {if (trigger == Trigger.DEATH) Activate(attack.Target);}
    public override void OnFighterDied(Fighter f, Fighter dead) {if (trigger == Trigger.FIGHTER_DIED) Activate(f);}
    public override void OnHeal(Fighter f) {if (trigger == Trigger.HEALED) Activate(f);}
    public override void MonsterSummoned(Fighter f, Fighter newFighter) {if (trigger == Trigger.MONSTER_SUMMONED) Activate(f);}

    public override string GetDescription()
    {
        return string.Format(description, damage, numOfTriggers, trigger);
    }
}
