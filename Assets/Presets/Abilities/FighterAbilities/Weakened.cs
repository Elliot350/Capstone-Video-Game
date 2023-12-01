using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weakened", menuName = "Abilities/Fighter/Weakened")]
public class Weakened : FighterAbility
{
    [SerializeField] private float extraDamage;
    [SerializeField] private List<Tag> sources;

    public override void OnTakenDamage(Damage attack)
    {
        // If its null or there is no tags in the list, then there is always extra damage applied
        if (sources == null || sources.Count == 0)
        {
            attack.baseDamage += extraDamage;
            return;
        }
        
        // Else this means we need to check if the source has contains a tag in the sources, if there is one, add the damage and return
        foreach (Tag t in sources)
        {
            if (attack.source.HasTag(t))
            {
                attack.baseDamage += extraDamage;
                return;
            }
        }
    }

    public override string GetDescription()
    {
        return string.Format(description, extraDamage, Tag.FormatTags(sources));
    }
}
