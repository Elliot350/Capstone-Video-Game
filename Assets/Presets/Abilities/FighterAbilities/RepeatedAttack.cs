using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RepeatedAttack", menuName = "Abilities/Fighter/Repeated Attack")]
public class RepeatedAttack : FighterAbility
{
    enum Target
    {
        FIRST,
        LAST,
        RANDOM,
        ALL
    }

    [SerializeField] private Target target;
    [SerializeField] private int numOfAttacks;

    public override List<Fighter> DecideTargets(List<Fighter> fighters)
    {
        List<Fighter> targets = new();
        for (int i = 0; i < numOfAttacks; i++)
        {    
            switch (target)
            {
                case Target.FIRST:
                    targets.Add(fighters[0]);
                    break;
                case Target.LAST:
                    targets.Add(fighters[^1]);
                    break;
                case Target.RANDOM:
                    targets.Add(fighters[Random.Range(0, fighters.Count)]);
                    break;
                // TODO: Not sure if this works, check before deleting HydraStrike
                case Target.ALL:
                    targets.AddRange(fighters);
                    break;
                default:
                    break;
            }
        }
        
        return targets;
    }

    public override int ModifiesTargets()
    {
        return ADDS_ATTACKS;
    }

    public override string GetDescription()
    {
        return string.Format(description);
    }
}
