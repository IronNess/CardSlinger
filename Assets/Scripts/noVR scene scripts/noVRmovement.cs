using UnityEngine;

public class noVRmovement : MonoBehaviour
{
    public CharacterController characterController;
    public float playerSpeed = 5.0f;

    private Vector3 playerVelocity;
    private float gravity = -9.81f;
    private bool grounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 forward = transform.forward * Input.GetAxis("Vertical");
        Vector3 horizontal = transform.right * Input.GetAxis("Horizontal");

        playerVelocity = Vector3.ClampMagnitude(forward + horizontal, 1) * playerSpeed;

        grounded = characterController.isGrounded;
        if (!grounded)
        {
            playerVelocity.y += gravity;
        }
        else
        {
            playerVelocity.y = 0;
        }

        characterController.Move(playerVelocity * Time.deltaTime);
    }
}
