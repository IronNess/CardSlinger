using UnityEngine;

public class cameraMovementNoVR : MonoBehaviour
{
    [Header("Settings")]
    public float mouseSensitivity = 100f; // Mouse sensitivity multiplier
    public Transform playerPos;          // Reference to the player body for horizontal rotation
    public bool lockCursor = true;        // Lock and hide cursor

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Lock and hide the cursor if enabled
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse movement input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Apply vertical rotation to the camera
        transform.Rotate(-mouseY, 0, 0);

        // Apply horizontal rotation to the player body
        playerPos.Rotate(Vector3.up * mouseX);        
    }
}
