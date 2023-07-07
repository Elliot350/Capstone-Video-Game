using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VenusFlytrap", menuName = "Abilities/Fighter/Venus Flytrap")]
public class VenusFlytrap : FighterAbility
{
    [SerializeField] private Tag tagToConsume;
    private List<Fighter> eatenFighters;

    public override void BattleStart(Fighter f)
    {
        Debug.Log("Start");
        eatenFighters = new List<Fighter>();
        foreach (Fighter ally in FightManager.GetInstance().GetAllies(f))
        {
            if (ally.HasTag(tagToConsume))
            {
                Debug.Log($"Eating {ally.GetName()}");
                eatenFighters.Add(ally);
                FightManager.GetInstance().AddAction(new Die(ally, new Damage(f, ally, 0f)));
            }
        }
    }

    public override void OnDeath(Damage attack)
    {
        Debug.Log("Death");
        foreach (Fighter fighter in eatenFighters)
        {
            FightManager.GetInstance().AddAction(new Revive(fighter));
        }
    }

    public override string GetDescription()
    {
        return string.Format(description, tagToConsume.Format());
    }
}
