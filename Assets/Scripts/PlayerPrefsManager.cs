using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    private static float playerPrefsVersion = 1f;
    public static bool hasSeenTutorial;

    public static void Save()
    {
        PlayerPrefs.SetFloat("version", playerPrefsVersion);
        PlayerPrefs.SetInt("seenTutorial", hasSeenTutorial ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static void Load()
    {
        if (PlayerPrefs.GetFloat("version", 0f) != playerPrefsVersion) Debug.LogWarning($"PlayerPrefs version ({PlayerPrefs.GetFloat("version")}) doesn't match current version ({playerPrefsVersion})");
        hasSeenTutorial = PlayerPrefs.GetInt("seenTutorial", 0) == 1;
    }

    public static void Delete()
    {
        PlayerPrefs.DeleteAll();
    }
}
