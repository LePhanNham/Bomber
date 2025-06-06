using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance { get; private set; }

    private const string UnlockKey = "UnlockedIndex";

    private List<string> levelScenes = new List<string>();
    public List<string> LevelScenes => levelScenes;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            if (name.StartsWith("Level"))
            {
                levelScenes.Add(name);
            }
        }
    }

    public bool IsUnlocked(string scene)
    {
        int index = levelScenes.IndexOf(scene);
        if (index < 0) return false;
        return index <= PlayerPrefs.GetInt(UnlockKey, 0);
    }

    public void UnlockNext(string currentScene)
    {
        int currentIndex = levelScenes.IndexOf(currentScene);
        int unlocked = PlayerPrefs.GetInt(UnlockKey, 0);
        if (currentIndex >= unlocked && currentIndex + 1 < levelScenes.Count)
        {
            PlayerPrefs.SetInt(UnlockKey, currentIndex + 1);
            PlayerPrefs.Save();
        }
    }
}
