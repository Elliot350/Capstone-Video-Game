using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VenusFlytrap", menuName = "Abilities/Fighter/Venus Flytrap")]
public class VenusFlytrap : FighterAbility
{
    [SerializeField] private Tag tagToConsume;
    private List<Fighter> targetFighters;

    public override void BattleStart(Fighter f)
    {
        Debug.Log("Start");
        targetFighters = new List<Fighter>();
        
        foreach (Fighter ally in FightManager.GetInstance().GetAllies(f))
        {
            if (ally.HasTag(tagToConsume))
            {
                Debug.Log($"Eating {ally.GetName()}");
                targetFighters.Add(ally);
                FightManager.GetInstance().AddAction(new PlayAnimation(ally, animationTrigger));
                // FightManager.GetInstance().AddAction(new Die(ally, new Damage(f, ally, 0f)));
            }
        }

        FightManager.GetInstance().AddAction(new Delay(1f));

        foreach (Fighter target in targetFighters)
        {
            FightManager.GetInstance().AddAction(new ContinueAnimation(target, animationTrigger));
            // if (target.GetEffect(animationTrigger) != null) FightManager.GetInstance().AddAction(new ContinueAnimation(target, animationTrigger2, target.GetEffect(animationTrigger)));
            FightManager.GetInstance().AddAction(new Die(target, new Damage(f, target, 0f)));
        }
    }

    public override void OnDeath(Damage attack)
    {
        Debug.Log("Death");
        foreach (Fighter fighter in targetFighters)
        {
            FightManager.GetInstance().AddAction(new Revive(fighter));
        }
    }

    public override string GetDescription()
    {
        return string.Format(description, tagToConsume.Format());
    }
}
