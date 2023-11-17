using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Healer", menuName = "Abilities/Fighter/Healer")]
public class Healer : TriggeredFighterAbility
{
    private enum Target {
        RANDOM,
        RANDOM_OTHER,
        ALL,
        SELF
    }
    [SerializeField] float healAmount;
    [SerializeField] private Target healTarget;

    protected override void Activate(Fighter self)
    {
        List<Fighter> allies;
        switch (healTarget)
        {
            case Target.RANDOM:
                allies = FightManager.GetInstance().GetTeam(self);
                FightManager.GetInstance().AddAction(new Heal(allies[Random.Range(0, allies.Count)], healAmount));
                break;
            case Target.RANDOM_OTHER:
                allies = FightManager.GetInstance().GetAllies(self);
                FightManager.GetInstance().AddAction(new Heal(allies[Random.Range(0, allies.Count)], healAmount));
                break;
            case Target.ALL:
                allies = FightManager.GetInstance().GetTeam(self);
                foreach (Fighter f in allies)
                    FightManager.GetInstance().AddAction(new Heal(f, healAmount));
                break;
            case Target.SELF:
                FightManager.GetInstance().AddAction(new Heal(self, healAmount));
                break;
        
        }
    }
 
    public override string GetDescription()
    {
        return string.Format(description, healAmount);
    }
}
