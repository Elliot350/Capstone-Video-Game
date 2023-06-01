using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TrapAbility : Ability
{
    [SerializeField] protected float chance;

    protected bool CheckChance() {return Random.Range(0f, 1f) <= chance;}
    public virtual void PartyEntered(Party party) {}
    public virtual void PartyExited(Party party) {}
}
