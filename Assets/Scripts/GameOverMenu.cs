using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public void MainMenu()
    {
        CustomSceneManager.OpenMainMenu();
    }

    public void PlayAgain()
    {
        CustomSceneManager.OpenGame();
    }
}
