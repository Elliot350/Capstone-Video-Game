using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TriggeredRoomAbility : RoomAbility
{
    protected enum Trigger {
        START_BATTLE,
        END_BATTLE,
        MONSTER_DIED,
        HERO_DIED,
        MONSTER_SUMMONED,
        HERO_SUMMONED
    }

    [SerializeField] protected List<Trigger> triggers;

    protected abstract void Activate(Room self);
    protected abstract void Activate(Room self, Fighter f);

    public override void BattleStart(Room r, List<Fighter> monsters, List<Fighter> heroes) {if (triggers.Contains(Trigger.START_BATTLE)) Activate(r);}
    public override void BattleEnd(Room r, List<Fighter> monsters, List<Fighter> heroes) {if (triggers.Contains(Trigger.END_BATTLE)) Activate(r);}
    public override void OnFighterDied(Room r, Fighter f) {if ((f.IsMonster && triggers.Contains(Trigger.MONSTER_DIED)) || (!f.IsMonster && triggers.Contains(Trigger.HERO_DIED))) Activate(r, f);}
    public override void FighterSummoned(Room r, Fighter f) {if ((f.IsMonster && triggers.Contains(Trigger.MONSTER_SUMMONED)) || (!f.IsMonster && triggers.Contains(Trigger.HERO_SUMMONED))) Activate(r, f);}
}
