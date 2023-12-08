using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationButton : MonoBehaviour
{
    public LocationData location;  

    public void Pressed()
    {
        if (location == null) 
        {
            Debug.Log($"Was pressed, but don't have a location!");
            return;
        }
        Debug.Log($"{location.name} is pressed");
        GameManager.GetInstance().SetLocation(location);
    }

    public LocationData GetData() 
    {
        return location;
    }
}
