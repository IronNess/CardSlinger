using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class battleClicked : MonoBehaviour
{
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
        Debug.Log("clicked");
        // need to change to load random scene
        SceneManager.LoadScene("Scenes/main");
    }

    private void OnDestroy() // avoids memory leaks
    {
        if(button != null)
        {
            button.onClick.RemoveListener(clicked);
        }
    }
}
