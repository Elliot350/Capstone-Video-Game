using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tag", menuName = "Tags/New Tag")]
public class Tag : ScriptableObject
{
    public string Name => name;
    public Color color = Color.white;

    public string FormatTag() 
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + name + "</color>";
    }

    public static string FormatTags(List<Tag> tags)
    {
        string formattedTags = tags[0].FormatTag();
        for (int i = 1; i < tags.Count; i++)
            formattedTags += ", " + tags[i].FormatTag();
        return formattedTags;
    }
}
