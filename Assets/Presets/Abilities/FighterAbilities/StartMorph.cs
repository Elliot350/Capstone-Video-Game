using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StartMorph", menuName = "Abilities/Fighter/Start Morph")]
public class StartMorph : FighterAbility
{
    public override void OnBattleStarted(Fighter f)
    {
        f.SetType(GameManager.GetInstance().GetRandomMonster());
    }

    public override string GetDescription()
    {
        return string.Format(description);
    }
}
