using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PermanentAbilities", menuName = "Abilities/Fighter/Permanent Abilities")]
public class PermanentAbilities : FighterAbility
{
    private enum Coverage 
    {
        ALL,
        ON_LIST
    }

    [SerializeField] private Coverage coverage;
    [SerializeField] private List<FighterAbility> protectedAbilities;

    public override bool CanRemoveAbility(FighterAbility ability)
    {
        switch (coverage)
        {
            case Coverage.ALL:
                return false;
            // TODO: I don't think this will work because the abilities are being instantiated
            case Coverage.ON_LIST:
                Debug.Log($"Looking for: {ability}");
                foreach (FighterAbility a in protectedAbilities)
                    Debug.Log($"{protectedAbilities.IndexOf(a)}: {a}");
                return !protectedAbilities.Contains(ability);
            default:
                return base.CanRemoveAbility(ability);
        }
    }

    public override string GetDescription()
    {
        return string.Format(description);
    }
}
