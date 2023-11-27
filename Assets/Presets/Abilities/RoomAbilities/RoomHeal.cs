using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomHeal", menuName = "Abilities/Room/Heal")]
public class RoomHeal : TriggeredRoomAbility
{
    protected enum Target {
        MONSTERS,
        HEROES,
        INTERACTED,
        ALL,
        RANDOM_MONSTER,
        RANDOM_HERO
    }

    [SerializeField] private float healAmount;
    [SerializeField] private Target target;


    public override string GetDescription()
    {
        return string.Format(description, healAmount);
    }

    protected override void Activate(Room self)
    {
        switch (target) {
            case Target.MONSTERS:
                foreach (Fighter f in FightManager.GetInstance().GetMonsters())
                    FightManager.GetInstance().AddAction(new Heal(f, healAmount));
                break;
            case Target.HEROES:
                foreach (Fighter f in FightManager.GetInstance().GetHeroes())
                    FightManager.GetInstance().AddAction(new Heal(f, healAmount));
                break;
            case Target.ALL:
                foreach (Fighter f in FightManager.GetInstance().GetFighters())
                    FightManager.GetInstance().AddAction(new Heal(f, healAmount));
                break;
            case Target.RANDOM_MONSTER:
                List<Fighter> monsters = FightManager.GetInstance().GetMonsters();
                FightManager.GetInstance().AddAction(new Heal(monsters[Random.Range(0, monsters.Count)], healAmount));
                break;
            case Target.RANDOM_HERO:
                List<Fighter> heroes = FightManager.GetInstance().GetHeroes();
                FightManager.GetInstance().AddAction(new Heal(heroes[Random.Range(0, heroes.Count)], healAmount));
                break;
            
        }
    }

    protected override void Activate(Room self, Fighter f)
    {
        if (target == Target.INTERACTED) FightManager.GetInstance().AddAction(new Heal(f, healAmount));
        else Activate(self);
    }
}
