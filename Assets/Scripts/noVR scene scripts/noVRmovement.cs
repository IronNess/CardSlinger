using UnityEngine;
using static UnityEngine.XR.OpenXR.Features.Interactions.HandInteractionProfile;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class noVRmovement : MonoBehaviour
{
    public CharacterController characterController;
    public float playerSpeed = 5.0f;

    public Transform cameraTransform;

    [Header("Deck")]
    public DeckManager deckManager;
    public float Force = 100.0f;

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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("1");

            // Ask DeckManager for the next card prefab
            GameObject cardPrefab = deckManager.DrawCard();

            if (cardPrefab == null)
                return;

            Debug.Log("2");

            Vector3 position = transform.position + new Vector3(0, 1, 0);

            // Spawn the card into the world
            GameObject spawnedCard = Instantiate(cardPrefab, position, cameraTransform.rotation);

            Rigidbody rb = spawnedCard.GetComponent<Rigidbody>();

            if (rb == null)
                return;

            Debug.Log("3");

            rb.AddForce(spawnedCard.transform.forward * Force, ForceMode.Force);

            Debug.Log("4");
        }
    }
}
