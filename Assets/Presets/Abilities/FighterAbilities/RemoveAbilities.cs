using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RemoveAbilities", menuName = "Abilities/Fighter/Remove Abilities")]
public class RemoveAbilities : TriggeredFighterAbility
{
    private enum Target 
    {
        RANDOM_ENEMY,
        RANDOM_ALLY
    }

    [SerializeField] private Target target;

    protected override void Activate(Fighter self)
    {
        Fighter targetFighter = null;
        switch (target) 
        {
            case Target.RANDOM_ENEMY:
                targetFighter = FightManager.GetInstance().GetEnemies(self)[Random.Range(0, FightManager.GetInstance().GetEnemies(self).Count)];
                break;
            case Target.RANDOM_ALLY:
                targetFighter = FightManager.GetInstance().GetAllies(self)[Random.Range(0, FightManager.GetInstance().GetAllies(self).Count)];
                break;
        }
        if (targetFighter != null) 
        {
            foreach (FighterAbility a in targetFighter.GetAbilities()) 
            {
                FightManager.GetInstance().AddAction(new RemoveAbility(targetFighter, a));
            }
        }
    }
 
    public override string GetDescription()
    {
        return string.Format(description);
    }
}
