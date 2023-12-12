using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StartMorph", menuName = "Abilities/Fighter/Start Morph")]
public class StartMorph : FighterAbility
{
    public override void OnStartBattle(Fighter f)
    {
        FightManager.GetInstance().AddAction(new Morph(
            f, 
            GameManager.GetInstance().GetRandomMonster((m) => m != f.GetFighterType())
            ));
    }

    public override string GetDescription()
    {
        return string.Format(description);
    }
}
