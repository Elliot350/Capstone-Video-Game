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
        SELF,
        LOWEST_HEALTH_ALLY
    }

    private enum HealType {
        VALUE,
        MULTIPLIER_OF_TARGET_HEALTH
    }

    [SerializeField] float healValue;
    [SerializeField] private HealType healType;
    [SerializeField] private Target healTarget;


    protected override void Activate(Fighter self)
    {
        List<Fighter> allies;
        switch (healTarget)
        {
            case Target.RANDOM:
                allies = FightManager.GetInstance().GetTeam(self);
                Heal(allies[Random.Range(0, allies.Count)]);
                break;
            case Target.RANDOM_OTHER:
                allies = FightManager.GetInstance().GetAllies(self);
                Heal(allies[Random.Range(0, allies.Count)]);
                break;
            case Target.ALL:
                allies = FightManager.GetInstance().GetTeam(self);
                foreach (Fighter f in allies)
                    Heal(f);
                break;
            case Target.SELF:
                Heal(self);
                break;
            case Target.LOWEST_HEALTH_ALLY:
                allies = FightManager.GetInstance().GetAllies(self);
                int lowest = 0;
                for (int i = 0; i < allies.Count; i++)
                    if (allies[i].GetHealth() < allies[lowest].GetHealth() && allies[i].GetHealth() < allies[i].GetMaxHealth())
                        lowest = i;
                Heal(allies[lowest]);
                break;
        
        }
    }

    private void Heal(Fighter f)
    {
        switch (healType)
        {
            case HealType.VALUE:
                FightManager.GetInstance().AddAction(new Heal(f, healValue));
                break;
            case HealType.MULTIPLIER_OF_TARGET_HEALTH:
                FightManager.GetInstance().AddAction(new Heal(f, healValue * f.GetMaxHealth()));
                break;
        }
    }
 
    public override string GetDescription()
    {
        return string.Format(description, healValue * (healType == HealType.VALUE ? 1 : 100));
    }
}
