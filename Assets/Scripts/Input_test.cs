using UnityEngine;
using UnityEngine.InputSystem;

public class Input_test : MonoBehaviour
{
    public InputActionProperty actionProperty;
    public InputActionProperty actionPropertyButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float value = actionProperty.action.ReadValue<float>();
        Debug.Log("Value: " + value);

        bool button = actionPropertyButton.action.IsPressed();
        Debug.Log("Button: " + button);
    }
}
