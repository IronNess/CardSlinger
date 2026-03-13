using UnityEngine;

public class cameraMovementNoVR : MonoBehaviour
{
    [Header("Settings")]
    public float mouseSensitivity = 100f; // Mouse sensitivity multiplier
    public Transform cameraPos;          // Reference to the player body for horizontal rotation
    public bool lockCursor = true;        // Lock and hide cursor

    [Header("Vertical Rotation Limits")]
    public float minY = -80f; // Minimum vertical angle
    public float maxY = 80f;  // Maximum vertical angle

    private float xRotation = 0f; // Tracks vertical rotation
    private float yRotation = 0f; // Tracks horizontal rotation

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

        // Adjust vertical rotation (inverted Y-axis)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minY, maxY);

        yRotation -= mouseX;

        // Apply vertical rotation to the camera
        //transform.rotation = Quaternion.Euler(xRotation, 0f, 0f);

        

        // Apply horizontal rotation to the player body
        cameraPos.Rotate(Vector3.up * yRotation);        
    }
}
