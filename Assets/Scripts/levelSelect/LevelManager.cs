using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private bool levelComplete = false;

    void Update()
    {
        if (levelComplete)
            return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemies.Length == 0)
        {
            WinLevel();
        }
    }

    void WinLevel()
    {
        levelComplete = true;
        Debug.Log("Level Complete!");

        if (GameProgress.Instance != null)
        {
            GameProgress.Instance.AdvanceLevel();
        }

        SceneManager.LoadScene("shop");
    }
}