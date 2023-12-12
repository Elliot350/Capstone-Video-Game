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
        RANDOM_ENEMY,
        EVERY_ENEMY
    }
    [SerializeField] private Target target;

    protected override void Activate(Fighter self)
    {
        Fighter abilityTarget = null;
        List<Fighter> enemies;
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
                enemies = FightManager.GetInstance().GetEnemies(self);
                if (enemies.Count == 0) return;
                abilityTarget = enemies[Random.Range(0, enemies.Count)];
                break;
            case Target.EVERY_ENEMY:
                enemies = FightManager.GetInstance().GetEnemies(self);
                foreach (Fighter f in enemies)
                    FightManager.GetInstance().AddAction(new AddAbility(f, ability));
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
            FightManager.GetInstance().AddAction(new AddAbility(attack.target, Instantiate<FighterAbility>(ability)));
        else if (triggers.Contains(Trigger.ATTACK))
            Activate(attack.source);
    }

    public override void OnTakenDamage(Damage attack)
    {
        if (target == Target.TARGET && triggers.Contains(Trigger.TAKEN_DAMAGE))
            FightManager.GetInstance().AddAction(new AddAbility(attack.source, Instantiate<FighterAbility>(ability)));
        else if (triggers.Contains(Trigger.TAKEN_DAMAGE))
            Activate(attack.target);
    }

    public override void OnDeath(Damage attack)
    {
        if (target == Target.TARGET && triggers.Contains(Trigger.DEATH))
            FightManager.GetInstance().AddAction(new AddAbility(attack.source, Instantiate<FighterAbility>(ability)));
        else if (triggers.Contains(Trigger.DEATH))
            Activate(attack.target);
    }

    public override void OnFighterSummoned(Fighter f, Fighter newFighter)
    {
        if ((!newFighter.isMonster && triggers.Contains(Trigger.HERO_SUMMONED)) || (newFighter.isMonster && triggers.Contains(Trigger.MONSTER_SUMMONED))) ApplyAbilityToSummoned(f, newFighter);
        // if (!newFighter.IsMonster && triggers.Contains(Trigger.HERO_SUMMONED) && f.IsMonster && target == Target.SUMMONED_ENEMY) FightManager.GetInstance().AddAction(new AddAbility(newFighter, ability));
    }

    private void ApplyAbilityToSummoned(Fighter f, Fighter newFighter)
    {
        if (target == Target.EVERY_ENEMY && FightManager.GetInstance().AreEnemies(f, newFighter))
        {
            FightManager.GetInstance().AddAction(new AddAbility(newFighter, ability));
        }
        else
        {
            Activate(f);
        }
    }

    public override string GetDescription()
    {
        return string.Format(description, ability.GetName(), ability.GetAbility());
    }
}
