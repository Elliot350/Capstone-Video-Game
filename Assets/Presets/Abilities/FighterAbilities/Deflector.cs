using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Deflector", menuName = "Abilities/Fighter/Deflector")]
public class Deflector : FighterAbility
{
    // TODO: Bug where the monster gets removed from the lists but not order
    public override void OnTakenDamage(Damage attack)
    {
        Debug.Log($"Trying to deflect");

        List<Fighter> allies = new List<Fighter>(FightManager.GetInstance().GetAllies(attack.target));
        allies.RemoveAll((f) => f.GetFighterType() == attack.target.GetFighterType());
        // for (int i = allies.Count - 1; i >= 0; i--)
        // {
        //     if (allies[i].GetFighterType() == attack.target.GetFighterType())
        //         allies.RemoveAt(i);
        // }
        if (allies.Count == 0)
        {
            Debug.Log($"Fails");
            return;
        }

        
        // Damage damage = new Damage(allies[Random.Range(0, allies.Count)], attack);
        FightManager.GetInstance().AddAction(new TakeDamage(Damage.RetargetDamage(attack, allies[Random.Range(0, allies.Count)])));
        Debug.Log($"Shwang!");
        attack.baseDamage = 0;
    }

    public override string GetDescription()
    {
        return string.Format(description);
    }
}
