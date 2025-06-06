using UnityEngine;

public enum Difficulty
{
    Easy,
    Normal,
    Hard
}

public class GameSettings : MonoBehaviour
{
    public static GameSettings Instance { get; private set; }

    public Difficulty difficulty = Difficulty.Normal;
    public string selectedMap;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
