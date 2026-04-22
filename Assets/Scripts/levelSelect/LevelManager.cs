using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private bool levelComplete = false;

    public GameObject levelSelect;

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

        //SceneManager.LoadScene("shop");
        GameObject playerInteractor = GameObject.Find("VR Player/Camera Offset/Right Hand/Ray Interactor");
        if (playerInteractor != null)
        {
            playerInteractor.SetActive(true);
            levelSelect.SetActive(true);
        }
        else
        {
            Debug.Log("Cant find interactor");
        }
    }
}