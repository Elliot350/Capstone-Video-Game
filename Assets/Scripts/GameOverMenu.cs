using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public void MainMenu()
    {
        GameManager.GetInstance().GoToMainMenu();
    }

    public void PlayAgain()
    {
        GameManager.GetInstance().PlayButtonPressed();
    }
}
