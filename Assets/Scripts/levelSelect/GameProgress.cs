using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Instance;

    public int currentLevelIndex = 1;

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

    public void AdvanceLevel()
    {
        currentLevelIndex++;
    }

    public string GetNextLevelSceneName()
    {
        return "Level" + currentLevelIndex;
    }
}

// This allows the shop to recognise what level is next after exiting. 