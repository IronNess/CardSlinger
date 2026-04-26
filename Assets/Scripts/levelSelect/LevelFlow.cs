using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFlow : MonoBehaviour
{
    public string nextScene;
    [SerializeField] private string homeScene = "Home";

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextScene);
    }

    public void LoadHome()
    {
        SceneManager.LoadScene(homeScene);
    }
}