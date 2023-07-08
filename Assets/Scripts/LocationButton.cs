using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LocationButton : MonoBehaviour
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private Sprite defaultImage, selectedImage;
    [SerializeField] private float distance;   

    // Update is called once per frame
    void Update()
    {
        Debug.Log($"Mouse: {Input.mousePosition}, {Camera.main.ScreenToWorldPoint(Input.mousePosition)}, {Camera.main.ScreenToViewportPoint(Input.mousePosition)}, distance: {GetDistance()}");
    }

    private float GetDistance()
    {
        // Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // return Vector3.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        return 0f;
    }
}
