using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : Fighter
{
    protected override void SetTypes()
    {
        IsMonster = true;
        IsBoss = false;
    }
}
