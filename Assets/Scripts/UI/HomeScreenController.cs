using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the Home screen button actions.
/// Attach this to a GameObject in the home scene and wire buttons via OnClick.
/// </summary>
public class HomeScreenController : MonoBehaviour
{
    [Header("Scene Flow")]
    [SerializeField] private string playSceneName = "levelSelect";
    [SerializeField] private string fallbackPlaySceneName = "levelSelect";

    public void Play()
    {
        string sceneToLoad = playSceneName;

        // Backward compatibility: older scene instances may still have "main" serialized.
        if (string.Equals(sceneToLoad, "main", System.StringComparison.OrdinalIgnoreCase))
        {
            sceneToLoad = fallbackPlaySceneName;
        }

        if (string.IsNullOrWhiteSpace(sceneToLoad))
        {
            Debug.LogError("HomeScreenController play scene is not set.");
            return;
        }

        SceneManager.LoadScene(sceneToLoad);
    }

    public void Exit()
    {
        Debug.Log("Exit requested from home screen.");
        Application.Quit();
    }
}
