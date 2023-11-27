using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [SerializeField] protected string abilityName;
    [SerializeField] protected string description;

    public string GetName() { return abilityName; }
    public abstract string GetDescription();
    public virtual string GetAbility() { return $"[{GetName()}] - ({GetDescription()})"; }

    public static string GetDescriptionFromList(List<Ability> abilities)
    {
        if (abilities.Count == 0)
            return "<i>No abilities</i>";
        string text = "";
        foreach (Ability a in abilities)
            text += a.GetAbility() + "\n";
        return text;
    }

    public static string GetDescriptionFromList(List<RoomAbility> abilities)
    {
        List<Ability> plainAbilities = new List<Ability>(abilities);
        return GetDescriptionFromList(plainAbilities);
    }

    public static string GetDescriptionFromList(List<FighterAbility> abilities)
    {
        List<Ability> plainAbilities = new List<Ability>(abilities);
        return GetDescriptionFromList(plainAbilities);
    }

    public static string GetDescriptionFromList(List<TrapAbility> abilities)
    {
        List<Ability> plainAbilities = new List<Ability>(abilities);
        return GetDescriptionFromList(plainAbilities);
    }
}
