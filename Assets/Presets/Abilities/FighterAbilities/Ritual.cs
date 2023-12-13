using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ritual", menuName = "Abilities/Fighter/Ritual")]
public class Ritual : FighterAbility
{
    [SerializeField] private FighterBase newForm;

    public override void OnStartBattle(Fighter f)
    {
        FightManager.GetInstance().AddAction(new ConditionalAction(() => FightManager.GetInstance().GetBoss() != null, new Morph(f, newForm)));
    }

    public override string GetDescription()
    {
        return string.Format(description);
    }
}
