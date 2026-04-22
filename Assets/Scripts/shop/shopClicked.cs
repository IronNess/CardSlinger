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
        SceneManager.LoadScene("Scenes/shop");
    }

    private void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(clicked);
        }
    }
}