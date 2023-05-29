using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Vampirism", menuName = "Abilities/Fighter/Vampirism")]
public class Vampirism : Ability
{
    [SerializeField] private float healAmount;

    public override void OnAttack(Fighter f)
    {
        f.Heal(healAmount);
    }

    public override string GetDescription()
    {
        return string.Format(description, healAmount);
    }
}
