using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFlow : MonoBehaviour
{
    public string nextScene;

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(nextScene);
    }
}