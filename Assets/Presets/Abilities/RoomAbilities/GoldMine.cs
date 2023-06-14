using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GoldMine", menuName = "Abilities/Room/Gold Mine")]
public class GoldMine : RoomAbility
{
    public int goldAmount;
    public float timeDelay;

    public override void Periodic()
    {
        GameManager.GetInstance().GainMoney(goldAmount);
    }

    public override string GetDescription()
    {
        return string.Format(description, goldAmount, timeDelay, goldAmount/timeDelay);
    }
}
