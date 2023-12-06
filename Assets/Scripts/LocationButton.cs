using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationButton : MonoBehaviour
{
    // [SerializeField] private MainMenu mainMenu;
    // [SerializeField] private Sprite defaultImage, selectedImage;
    // [SerializeField] private Image image;
    // [SerializeField] private RectTransform rectTransform;
    // [SerializeField] private float distance;
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

    // Update is called once per frame
    // void Update()
    // {
    //     // Debug.Log($"{rectTransform.anchoredPosition} to {Input.mousePosition} = {GetDistance()}");
    //     if (GetDistance() < distance)
    //     {
    //         image.sprite = selectedImage;
    //     }
    //     else 
    //     {
    //         image.sprite = defaultImage;
    //     }
    // }

    // public void Check()
    // {
    //     if (GetDistance() < distance)
    //     {
    //         mainMenu.SetSelected(this);
    //     }
    // }

    // private float GetDistance()
    // {
    //     return Vector3.Distance(rectTransform.anchoredPosition, Input.mousePosition);
    // }

    public LocationData GetData() 
    {
        return location;
    }
}
