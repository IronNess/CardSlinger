using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopContinueButton : MonoBehaviour
{
    [SerializeField] private string homeScene = "Home";

    public void ContinueGame()
    {
        if (GameProgress.Instance == null)
        {
            Debug.LogError("GameProgress instance not found.");
            return;
        }

        string nextScene = GameProgress.Instance.GetNextLevelSceneName();
        Debug.Log("Trying to load: " + nextScene);
        SceneManager.LoadScene(nextScene);
    }

    public void LoadHome()
    {
        SceneManager.LoadScene(homeScene);
    }
}