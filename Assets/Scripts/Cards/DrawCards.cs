using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class DrawCards : MonoBehaviour
{
    [Header("Draw settings")]
    public DeckManager deckManager;          // Reference to the deck manager
    public XRDirectInteractor handInteractor; // The player's hand interactor
    public string deckTag = "Deck";          // Tag used to identify the deck object
    public InputActionProperty gripValue;    // Grip input from XR controller

    private bool touchingDeck;               // True if the hand is currently touching the deck
    private bool cardIsSpawned = false;      // Stops multiple cards spawning from one press
    private GameObject spawnedCard;          // Stores the card that was just created

    void Update()
    {
        // If hand is touching the deck, grip is pressed, and no card currently spawned -> draw a card
        if (touchingDeck && gripValue.action.IsPressed() && !cardIsSpawned)
        {
            cardIsSpawned = true;
            SpawnCard();
        }

        // Reset card spawn lock when grip is released
        if (gripValue.action.WasReleasedThisFrame())
        {
            cardIsSpawned = false;
        }
    }

    void SpawnCard()
    {
        // Ask DeckManager for the next card prefab
        GameObject cardPrefab = deckManager.DrawCard();

        if (cardPrefab == null)
            return;

        // Spawn the card into the world
        spawnedCard = Instantiate(cardPrefab);

        // Tell XR Interaction Toolkit to immediately place the spawned card into the player's hand
        IXRSelectInteractor interactor = handInteractor;
        IXRSelectInteractable interactable = spawnedCard.GetComponent<XRGrabInteractable>();

        if (handInteractor.interactionManager != null && interactable != null)
        {
            handInteractor.interactionManager.SelectEnter(interactor, interactable);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect when the player's hand touches the deck
        if (other.CompareTag(deckTag))
        {
            touchingDeck = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Detect when the player's hand leaves the deck
        if (other.CompareTag(deckTag))
        {
            touchingDeck = false;
        }
    }
}