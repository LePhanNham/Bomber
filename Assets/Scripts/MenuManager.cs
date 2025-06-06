using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject instructionsPanel;
    public GameObject settingsPanel;
    public GameObject mapPanel;
    public Dropdown mapDropdown;
    public Dropdown difficultyDropdown;

    private List<string> levelScenes = new List<string>();

    private void Start()
    {
        if (ProgressManager.Instance != null)
        {
            levelScenes = new List<string>(ProgressManager.Instance.LevelScenes);
        }
        PopulateMapDropdown();
    }

    private void PopulateMapDropdown()
    {
        if (mapDropdown == null)
            return;

        mapDropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (string scene in levelScenes)
        {
            if (ProgressManager.Instance == null || ProgressManager.Instance.IsUnlocked(scene))
            {
                options.Add(scene);
            }
        }
        if (options.Count > 0)
            mapDropdown.AddOptions(options);
    }

    public void Play()
    {
        if (mapDropdown == null || mapDropdown.options.Count == 0)
            return;
        string selected = mapDropdown.options[mapDropdown.value].text;
        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.selectedMap = selected;
            if (difficultyDropdown != null)
            {
                GameSettings.Instance.difficulty = (Difficulty)difficultyDropdown.value;
            }
        }
        SceneManager.LoadScene(selected);
    }

    public void ShowInstructions(bool visible)
    {
        if (instructionsPanel != null)
            instructionsPanel.SetActive(visible);
    }

    public void ShowSettings(bool visible)
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(visible);
    }

    public void ShowMapSelect(bool visible)
    {
        if (mapPanel != null)
            mapPanel.SetActive(visible);
        if (visible)
            PopulateMapDropdown();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
