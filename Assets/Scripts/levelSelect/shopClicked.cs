using UnityEngine;
using UnityEngine.UI;
public class shopClicked : MonoBehaviour
{
    public Button button;

    private void Awake()
    {
        if (button == null) // in case not assigned in editor
        {
            button = GetComponent<Button>();
        }

        button.onClick.AddListener(clicked);
    }

    private void clicked()
    {
        Debug.Log("shop clicked");
        // need to add load shop scene here
    }

    private void OnDestroy() // avoids memory leaks
    {
        if (button != null)
        {
            button.onClick.RemoveListener(clicked);
        }
    }
}
