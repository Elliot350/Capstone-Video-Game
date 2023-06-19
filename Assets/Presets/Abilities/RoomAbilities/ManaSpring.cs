using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ManaSpring", menuName = "Abilities/Room/Mana Spring")]
public class ManaSpring : RoomAbility
{
    public int manaAmount;
    public float timeDelay;

    public override void Periodic()
    {
        GameManager.GetInstance().GainMana(manaAmount);
    }

    public override string GetDescription()
    {
        return string.Format(description, manaAmount, timeDelay, manaAmount/timeDelay);
    }
}
