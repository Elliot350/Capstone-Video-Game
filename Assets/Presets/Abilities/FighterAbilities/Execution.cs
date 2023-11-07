using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Execution", menuName = "Abilities/Fighter/Execution")]
public class Execution : FighterAbility
{
    [SerializeField] private float threshold;
    private Fighter targetedFighter;

    public override void OnAttack(Damage attack)
    {
        targetedFighter = attack.Target;
        FightManager.GetInstance().AddAction(new ConditionalAction(() => targetedFighter.GetHealth() <= threshold, new Die(targetedFighter)));
    }

    public override string GetDescription()
    {
        return string.Format(description, threshold);
    }
}
