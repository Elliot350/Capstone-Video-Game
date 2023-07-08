using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Animator animator;
    private string selectedLocation;

    private void Update() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(selectedLocation);
        }    
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        selectedLocation = location.locationName;
    }

}
