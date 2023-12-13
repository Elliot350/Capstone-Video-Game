using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    public static readonly int mainMenuScene = 0;
    public static readonly int gameScene = 1;
    public static readonly int gameOverScene = 2;

    public static void OpenMainMenu()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(mainMenuScene);
    }

    public static void OpenGame()
    {
        GameManager.GetInstance().LoadLocationData();
        GameManager.GetInstance().SetHealth(GameManager.GetInstance().GetMaxHealth());
        // AsyncOperation async = SceneManager.LoadSceneAsync(gameScene);
        SceneManager.LoadScene(gameScene);
    }

    public static void OpenGameOver()
    {
        PlayerPrefsManager.Save();
        AsyncOperation async = SceneManager.LoadSceneAsync(gameOverScene);
    }

    
}
