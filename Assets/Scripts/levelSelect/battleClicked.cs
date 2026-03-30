using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class battleClicked : MonoBehaviour
{
    // List of scene names to choose from (must match names in Build Settings)
    [SerializeField] private string[] sceneNames;
    public Button button;

    private void Awake()
    {
        if(button == null) // in case not assigned in editor
        {
            button = GetComponent<Button>();
        }

        button.onClick.AddListener(clicked);
    }

    private void clicked()
    {
        int index = Random.Range(0, sceneNames.Length);
        // need to change to load random scene
        SceneManager.LoadScene(sceneNames[index]);
    }

    private void OnDestroy() // avoids memory leaks
    {
        if(button != null)
        {
            button.onClick.RemoveListener(clicked);
        }
    }
}
