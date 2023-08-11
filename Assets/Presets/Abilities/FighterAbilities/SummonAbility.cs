using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Summon", menuName = "Abilities/Fighter/Summon")]
public class SummonAbility : TriggeredFighterAbility
{
    [SerializeField] private bool summonAll;
    [SerializeField] private List<FighterBase> fighters;
    [SerializeField] private List<FighterAbility> additionalAbilities;

    protected override void Activate(Fighter thisFighter)
    {
        if (fighters == null || fighters.Count == 0) return;

        if (summonAll)
        {
            // Summon each monster
            foreach (FighterBase f in fighters)
            {
                FightManager.GetInstance().AddAction(new Summon(thisFighter, f, additionalAbilities));
            }
        }
        else
        {
            // Summon a random monster
            FightManager.GetInstance().AddAction(new Summon(thisFighter, fighters[Random.Range(0, fighters.Count)], additionalAbilities));
        }
    }

    public override string GetDescription()
    {
        return description;
        // return string.Format(description);
        // if (summonAll)
        // {
        //     string monsterList = monsters[0].GetName();
        //     for (int i = 1; i < monsters.Count - 1; i++)
        //     {
        //         monsterList += ", " + monsters[i].GetName();
        //     }
        //     monsterList += "and " + monsters[monsters.Count - 1].GetName();
        // }
        // else 
        // {

        // }
    }
}
