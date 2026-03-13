using UnityEngine;

public class noVRmovement : MonoBehaviour
{
    public CharacterController characterController;
    public float playerSpeed = 5.0f;
    public Transform cameraPos;
    public Transform currentPos;

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
        currentPos.localRotation = Quaternion.Euler(cameraPos.rotation.x, cameraPos.rotation.y, 0f);

        //Vector3 forward = cameraPos.forward * Input.GetAxis("Vertical");
        //Vector3 horizontal = cameraPos.right * Input.GetAxis("Horizontal");

        //playerVelocity = (forward + horizontal) * playerSpeed;
        //playerVelocity = Vector3.ClampMagnitude(forward + horizontal, 1) * playerSpeed;


        playerVelocity = new Vector3(Input.GetAxis("Horizontal") * playerSpeed, 0, Input.GetAxis("Vertical") * playerSpeed);

        /*grounded = characterController.isGrounded;
        if (!grounded)
        {
            playerVelocity.y += gravity;
        }
        else
        {
            playerVelocity.y = 0;
        }*/

        characterController.Move(playerVelocity * Time.deltaTime);
    }
}
