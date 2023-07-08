using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocationButton : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private Sprite defaultImage, selectedImage;
    [SerializeField] private Image image;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float distance;
    public string locationName;  

    // Update is called once per frame
    void Update()
    {
        // Debug.Log($"{rectTransform.anchoredPosition} to {Input.mousePosition} = {GetDistance()}");
        if (GetDistance() < distance)
        {
            image.sprite = selectedImage;
            mainMenu.SetSelected(this);
        }
        else 
        {
            image.sprite = defaultImage;
        }
    }

    private float GetDistance()
    {
        return Vector3.Distance(rectTransform.anchoredPosition, Input.mousePosition);
    }
}
