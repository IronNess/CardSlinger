using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using static UnityEngine.Rendering.DebugUI;

public class DrawCards : MonoBehaviour
{
    [Header("Draw settings")]
    public GameObject cardPrefab;
    public XRDirectInteractor handInteractor; 
    public string deckTag = "Deck";
    public InputActionProperty gripValue;

    private bool touchingDeck;
    private bool cardIsSpawned = false;
    private GameObject spawnedCard;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (touchingDeck && gripValue.action.IsPressed() && !cardIsSpawned)
        {
            cardIsSpawned=true;
            SpawnCard();
        }
        if(gripValue.action.WasReleasedThisFrame())
        {
            cardIsSpawned=false;
        }
    }

    void SpawnCard()
    {
        //spawns card
        spawnedCard = Instantiate(cardPrefab);

        // Force grab using the new IXRSelectInteractor API (XRGrab interactable is obselete)
        IXRSelectInteractor interactor = handInteractor;
        IXRSelectInteractable interactable = spawnedCard.GetComponent<XRGrabInteractable>();

        if (handInteractor.interactionManager != null)
        {
            handInteractor.interactionManager.SelectEnter(interactor, interactable);
        }
    }

    // Detect when hand touches the deck
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(deckTag))
        {
            touchingDeck = true;
        }
    }

    // Detect when hand stops touching the deck
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(deckTag))
        {
            touchingDeck = false;
        }
    }
}
