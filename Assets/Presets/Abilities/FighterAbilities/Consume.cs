using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Consume", menuName = "Abilities/Fighter/Consume")]
public class Consume : FighterAbility
{
    [SerializeField] private int numOfTriggers;
    // Maybe make this a list so there can be multiple triggers?
    [SerializeField] private Trigger trigger;

    private void Activate(Fighter thisFighter)
    {
        List<Fighter> enemies = FightManager.GetInstance().GetAllies(thisFighter);
        if (enemies.Count == 0) return;
        Fighter target = enemies[Random.Range(0, enemies.Count)];
        float healthValue = target.GetHealth();
        float damageValue = target.GetDamage();

        FightManager.GetInstance().AddAction(new Die(target, new Damage(thisFighter, target, 0f)));
        FightManager.GetInstance().AddAction(new BuffMonster(thisFighter, healthValue, damageValue));
    }

    public override void BattleStart(Fighter f) {if (trigger == Trigger.START_BATTLE) Activate(f);}
    public override void BattleEnd(Fighter f) {if (trigger == Trigger.END_BATTLE) Activate(f);}
    public override void TurnStart(Fighter f) {if (trigger == Trigger.START_TURN) Activate(f);}
    public override void TurnEnd(Fighter f) {if (trigger == Trigger.END_TURN) Activate(f);}
    public override void OnAttack(Damage attack) {if (trigger == Trigger.ATTACK) Activate(attack.Source);}
    public override void OnTakenDamage(Damage attack) {if (trigger == Trigger.DAMAGED) Activate(attack.Target);}
    public override void OnDeath(Damage attack) {if (trigger == Trigger.DEATH) Activate(attack.Target);}
    public override void OnFighterDied(Fighter f, Fighter dead) {if (trigger == Trigger.MONSTER_DIED) Activate(f);}
    public override void OnHeal(Fighter f) {if (trigger == Trigger.HEALED) Activate(f);}

    public override string GetDescription()
    {
        return string.Format(description);
    }
}
