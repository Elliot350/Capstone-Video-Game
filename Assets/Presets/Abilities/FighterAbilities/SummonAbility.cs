using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Summon", menuName = "Abilities/Fighter/Summon")]
public class SummonAbility : FighterAbility
{
    [SerializeField] private Trigger trigger;
    [SerializeField] private List<MonsterBase> monsters;
    [SerializeField] private bool summonAll;

    public void Activate(Fighter thisFighter)
    {
        if (monsters == null || monsters.Count == 0) return;

        if (summonAll)
        {
            // Summon each monster
            foreach (MonsterBase m in monsters)
            {
                FightManager.GetInstance().AddAction(new Summon(thisFighter, m));
            }
        }
        else
        {
            // Summon a random monster
            FightManager.GetInstance().AddAction(new Summon(thisFighter, monsters[Random.Range(0, monsters.Count)]));
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
        return string.Format(description);
        // if (summonAll)
        // {
        //     string monsterList = monsters[0].GetName();
        //     for (int i = 1; i < monsters.Count - 1; i++)
        //     {
        //         monsterList += ", " + monsters[i].GetName();
        //     }
        //     monsterList += "and " + monsters[monsters.Count - 1].GetName();
        // }
        // else 
        // {

        // }
    }
}
