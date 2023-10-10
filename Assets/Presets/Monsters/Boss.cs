using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Monster
{
    protected override void SetTypes()
    {
        IsMonster = true;
        IsBoss = true;
    }
}
