using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator animator;
    private LocationData selectedLocation;
    [SerializeField] private List<LocationButton> locations;

    private void Update() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach (LocationButton lb in locations)
            {
                // lb.Check();
            }
        }
    }
    
    public void PlayGame()
    {
        GameManager.GetInstance().PlayButtonPressed();
    }

    public void OpenMap()
    {
        animator.SetTrigger("Map");
    }

    public void CloseMap()
    {
        animator.SetTrigger("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log($"Quitting...");
        Application.Quit();
    }

    public void SetSelected(LocationButton location)
    {
        selectedLocation = location.GetData();
    }

}
