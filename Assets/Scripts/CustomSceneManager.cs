using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    private static int mainMenuScene = 0;
    private static int gameScene = 1;
    private static int gameOverScene = 2;

    public static void OpenMainMenu()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(mainMenuScene);
        async.completed += (x) => {
            GameManager.GetInstance().SetLocation(null);
        };
    }

    public static void OpenGame()
    {
        GameManager.GetInstance().LoadLocationData();
        AsyncOperation async = SceneManager.LoadSceneAsync(gameScene);
        async.completed += (x) => {
            PlayerPrefsManager.Load();
            GameManager.GetInstance().InitializeManagers();
            GameManager.GetInstance().SetHealth(GameManager.GetInstance().GetMaxHealth());
            DungeonManager.GetInstance().PlaceEmpties();
            DungeonManager.GetInstance().PlaceBasicDungeon();
        };
    }

    public static void OpenGameOver()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(gameOverScene);
        async.completed += (x) => {

        };
    }

    
}
