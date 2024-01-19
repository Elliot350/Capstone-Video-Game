using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleStartTrigger", menuName = "Abilities/Room/Battle Start Retrigger")]
public class BattleStartTrigger : TriggeredRoomAbility
{
    private enum ActivateTargets 
    {
        MONSTERS,
        HEROES,
        ALL,
        ON_LIST
    }

    [SerializeField] private ActivateTargets targetType;
    [SerializeField] private List<FighterBase> fighterList;


    protected override void Activate(Room self)
    {
        List<Fighter> fighters = new List<Fighter>();
        switch (targetType) 
        {
            case ActivateTargets.MONSTERS:
                fighters.AddRange(FightManager.GetInstance().GetMonsters());
                break;
            case ActivateTargets.HEROES:
                fighters.AddRange(FightManager.GetInstance().GetHeroes());
                break;
            case ActivateTargets.ALL:
                fighters.AddRange(FightManager.GetInstance().GetFighters());
                break;
            // Add all of the fighters on the list to the targets
            case ActivateTargets.ON_LIST:
                foreach (Fighter f in FightManager.GetInstance().GetFighters())
                    if (fighterList.Contains(f.GetFighterType()))
                        fighters.Add(f);
                break;
        }
        foreach (Fighter f in fighters) {
            FightManager.GetInstance().AddAction(new BattleStart(f));
        }
    }

    protected override void Activate(Room self, Fighter f)
    {
        Activate(self);
    }
 
    public override string GetDescription()
    {
        return string.Format(description);
    }
}
