using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tag", menuName = "Other/New Tag")]
public class Tag : ScriptableObject
{
    public string Name => name;
    public Color color = Color.white;

    public string Format() 
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + name + "</color>";
    }

    public static string FormatTags(List<Tag> tags)
    {
        if (tags.Count == 0) return "";
        string formattedTags = tags[0].Format();
        for (int i = 1; i < tags.Count; i++)
            formattedTags += ", " + tags[i].Format();
        return formattedTags;
    }
}
