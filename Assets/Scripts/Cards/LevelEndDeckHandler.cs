using UnityEngine;
using UnityEngine.SceneManagement;


/// Handles deck preparation when leaving a level.

public class LevelEndDeckHandler : MonoBehaviour
{
    public void EndLevelAndGoToScene(string nextSceneName)
    {
        if (PersistentDeckState.Instance != null)
        {
            // Prepare deck for the next combat 
            PersistentDeckState.Instance.PrepareDeckForNextLevel();
        }

        SceneManager.LoadScene(nextSceneName);
    }

    public void StartNewRun(string firstSceneName)
    {
        if (PersistentDeckState.Instance != null)
        {
            PersistentDeckState.Instance.ResetDeckToStartingDeck();
        }

        SceneManager.LoadScene(firstSceneName);
    }
}