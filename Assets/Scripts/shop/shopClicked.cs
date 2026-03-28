using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class shopClicked : MonoBehaviour
{
    public Button button;

    private void Awake()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        if (button == null)
        {
            Debug.LogError("shopClicked: No Button found on " + gameObject.name);
            return;
        }

        button.onClick.AddListener(clicked);
        Debug.Log("shopClicked listener added on " + gameObject.name);
    }

    private void clicked()
    {
        Debug.Log("Continue button clicked");

        if (GameProgress.Instance == null)
        {
            Debug.LogError("GameProgress.Instance is NULL");
            return;
        }

        string nextLevel = GameProgress.Instance.GetNextLevelSceneName();
        Debug.Log("Trying to load scene: " + nextLevel);

        SceneManager.LoadScene(nextLevel);
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(clicked);
        }
    }
}