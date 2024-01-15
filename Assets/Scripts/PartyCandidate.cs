using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PartyCandidate
{
    public enum NumberComparison {
        NONE,
        LESS_THAN,
        MORE_THAN,
        EQUAL
    }

    public enum TagComparison {
        NONE,
        HAS_TAG,
        DOESNT_HAVE_TAG
    }

    public NumberComparison healthComparison;
    public float health;

    public NumberComparison damageComparison;
    public float damage;

    public NumberComparison tierComparison;
    public int tier;

    public TagComparison tagComparison;
    public Tag tag;

    public bool IsValid(HeroBase hero)
    {
        switch (healthComparison)
        {
            case NumberComparison.NONE:
                break;
            case NumberComparison.LESS_THAN:
                if (hero.GetMaxHealth() > health) return false;
                break;
            case NumberComparison.MORE_THAN:
                if (hero.GetMaxHealth() < health) return false;
                break;
            case NumberComparison.EQUAL:
                if (hero.GetMaxHealth() != health) return false;
                break;
        }
        
        switch (damageComparison)
        {
            case NumberComparison.NONE:
                break;
            case NumberComparison.LESS_THAN:
                if (hero.GetDamage() > damage) return false;
                break;
            case NumberComparison.MORE_THAN:
                if (hero.GetDamage() < damage) return false;
                break;
            case NumberComparison.EQUAL:
                if (hero.GetDamage() != damage) return false;
                break;
        }

        switch (tierComparison)
        {
            case NumberComparison.NONE:
                break;
            case NumberComparison.LESS_THAN:
                if (hero.GetTier() > tier) return false;
                break;
            case NumberComparison.MORE_THAN:
                if (hero.GetTier() < tier) return false;
                break;
            case NumberComparison.EQUAL:
                if (hero.GetTier() != tier) return false;
                break;
        }

        switch (tagComparison)
        {
            case TagComparison.NONE:
                break;
            case TagComparison.HAS_TAG:
                if (!hero.HasTag(tag)) return false;
                break;
            case TagComparison.DOESNT_HAVE_TAG:
                if (hero.HasTag(tag)) return false;
                break;
        }

        return true;
    }

    public override string ToString()
    {
        string str = "Requirements: ";
        switch (healthComparison)
        {
            case NumberComparison.NONE:
                break;
            case NumberComparison.LESS_THAN:
                str += $"Has less than {health} health, ";
                break;
            case NumberComparison.MORE_THAN:
                str += $"Has more than {health} health, ";
                break;
            case NumberComparison.EQUAL:
                str += $"Has {health} health";
                break;
        }

        switch (damageComparison)
        {
            case NumberComparison.NONE:
                break;
            case NumberComparison.LESS_THAN:
                str += $"Has less than {damage} damage, ";
                break;
            case NumberComparison.MORE_THAN:
                str += $"Has more than {damage} damage, ";
                break;
            case NumberComparison.EQUAL:
                str += $"Has {damage} damage, ";
                break;
        }

        switch (damageComparison)
        {
            case NumberComparison.NONE:
                break;
            case NumberComparison.LESS_THAN:
                str += $"Is less than tier {tier}, ";
                break;
            case NumberComparison.MORE_THAN:
                str += $"Is more than tier {tier}, ";
                break;
            case NumberComparison.EQUAL:
                str += $"Is tier {tier}, ";
                break;
        }

        switch (tagComparison)
        {
            case TagComparison.NONE:
                break;
            case TagComparison.HAS_TAG:
                str += $"Has {tag.Format()}";
                break;
            case TagComparison.DOESNT_HAVE_TAG:
                str += $"Doesn't have {tag.Format()}";
                break;
        }

        return str;
    }
}