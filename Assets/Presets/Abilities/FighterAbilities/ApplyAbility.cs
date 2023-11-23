using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ApplyAbility", menuName = "Abilities/Fighter/Apply Ability")]
public class ApplyAbility : TriggeredFighterAbility
{
    [SerializeField] private FighterAbility ability;
    private enum Target {
        SELF,
        TARGET,
        RANDOM_ALLY,
        RANDOM_ENEMY
    }
    [SerializeField] private Target target;

    protected override void Activate(Fighter self)
    {
        Fighter abilityTarget;
        switch (target)
        {
            case Target.SELF:
                abilityTarget = self;
                break;
            case Target.RANDOM_ALLY:
                // TODO: Check the allies at the time of applying the ability to remove any candidates that already have the ability
                List<Fighter> allies = FightManager.GetInstance().GetAllies(self);
                if (allies.Count == 0) return;
                abilityTarget = allies[Random.Range(0, allies.Count)];
                break;
            case Target.RANDOM_ENEMY:
                // TODO: Check the enemies at the time of applying the ability to remove any candidates that already have the ability
                List<Fighter> enemies = FightManager.GetInstance().GetEnemies(self);
                if (enemies.Count == 0) return;
                abilityTarget = enemies[Random.Range(0, enemies.Count)];
                break;
            default:
                abilityTarget = null;
                break;
        }
        if (abilityTarget != null) FightManager.GetInstance().AddAction(new AddAbility(abilityTarget, ability));
    }

    public override void OnAttack(Damage attack)
    {
        if (target == Target.TARGET && triggers.Contains(Trigger.ATTACK))
            FightManager.GetInstance().AddAction(new AddAbility(attack.Target, Instantiate<FighterAbility>(ability)));
        else if (triggers.Contains(Trigger.ATTACK))
            Activate(attack.Source);
    }

    public override void OnTakenDamage(Damage attack)
    {
        if (target == Target.TARGET && triggers.Contains(Trigger.DAMAGED))
            FightManager.GetInstance().AddAction(new AddAbility(attack.Source, Instantiate<FighterAbility>(ability)));
        else if (triggers.Contains(Trigger.DAMAGED))
            Activate(attack.Target);
    }

    public override void OnDeath(Damage attack)
    {
        if (target == Target.TARGET && triggers.Contains(Trigger.DEATH))
            FightManager.GetInstance().AddAction(new AddAbility(attack.Source, Instantiate<FighterAbility>(ability)));
        else if (triggers.Contains(Trigger.DEATH))
            Activate(attack.Target);
    }

    public override string GetDescription()
    {
        return string.Format(description, ability.GetName(), ability.GetAbility());
    }
}
