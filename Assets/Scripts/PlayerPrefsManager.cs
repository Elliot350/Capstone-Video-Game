using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    private static float playerPrefsVersion = 1f;
    public static bool seenTutorial;

    public static void Save()
    {
        PlayerPrefs.SetFloat("version", playerPrefsVersion);
        PlayerPrefs.SetInt("seenTutorial", seenTutorial ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static void Load()
    {
        if (PlayerPrefs.GetFloat("version") != playerPrefsVersion) Debug.LogWarning($"PlayerPrefs version doesn't match current version");
        if (PlayerPrefs.HasKey("seenTutorial")) seenTutorial = PlayerPrefs.GetInt("seenTutorial") == 1;
    }

    public static void Delete()
    {
        PlayerPrefs.DeleteAll();
    }
}
