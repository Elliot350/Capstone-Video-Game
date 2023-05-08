using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tag", menuName = "Tags/New Tag")]
public class Tag : ScriptableObject
{
    public string Name => name;
    public Color color;

    public string FormatTag() 
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" + name + "</color>";
    }
}
