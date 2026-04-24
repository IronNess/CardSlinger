using System.Collections.Generic;
using UnityEngine;

/// Stores the player's deck across scenes
/// Ness - Single object to survive scene changes and stores current draw pile, discard pile, starting deck. TO ADD - Scene DeckManager later

public class PersistentDeckState : MonoBehaviour
{
    public static PersistentDeckState Instance;

    [Header("Starting Deck")]
    public List<GameObject> startingDeck = new List<GameObject>();

    [Header("Current Run State")]
    public List<GameObject> drawPile = new List<GameObject>();
    public List<GameObject> discardPile = new List<GameObject>();

    private void Awake()
    {
        // Singleton pattern: only allow one persistent deck object
        /*if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);*/

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

            // If this is the first time the object exists, create a fresh run
            if (drawPile.Count != startingDeck.Count)
        {
            ResetDeckToStartingDeck();
        }
    }

   
    /// Resets the current run back to the starting deck.
    /// Use this when starting a brand new run.
  
    public void ResetDeckToStartingDeck()
    {
        drawPile.Clear();
        discardPile.Clear();

        drawPile.AddRange(startingDeck);
        Shuffle(drawPile);

        Debug.Log("Deck reset to starting deck.");
    }

   
    /// Draw the next card from the draw pile.
    /// If draw pile is empty, reshuffle discard into draw pile.
    
    public GameObject DrawCard()
    {
        if (drawPile.Count == 0)
        {
            if (discardPile.Count > 0)
            {
                ReshuffleDiscardIntoDraw();
            }
            else
            {
                Debug.LogWarning("No cards left in draw or discard pile.");
                return null;
            }
        }

        GameObject card = drawPile[0];
        drawPile.RemoveAt(0);
        return card;
    }

  
    /// Add a new card to the deck permanently.
    /// Useful when buying a new card from the shop.
     public void AddCard(GameObject cardPrefab)
    {
        startingDeck.Add(cardPrefab);
        Debug.Log("Added card to persistent deck: " + cardPrefab.name);
    }

   
    /// Remove a card permanently from the deck.
    
   
    public void RemoveCard(GameObject cardPrefab)
    {
        if (drawPile.Contains(cardPrefab))
        {
            drawPile.Remove(cardPrefab);
            return;
        }

        if (discardPile.Contains(cardPrefab))
        {
            discardPile.Remove(cardPrefab);
        }
    }

   
    /// Put a used card into the discard pile.
  
    public void DiscardCard(GameObject cardPrefab)
    {
        discardPile.Add(cardPrefab);
    }

   
    /// Call this when a level ends and want the next combat to begin
    /// with a fresh shuffled draw pile containing everything currently owned.
   
    public void PrepareDeckForNextLevel()
    {
        drawPile.AddRange(discardPile);
        discardPile.Clear();
        Shuffle(drawPile);

        Debug.Log("Deck prepared for next level.");
    }

    /// Move discard pile back into draw pile and shuffle.
   
    public void ReshuffleDiscardIntoDraw()
    {
        drawPile.Clear();
        discardPile.Clear();

        drawPile.AddRange(startingDeck);
        Shuffle(drawPile);

        Debug.Log("Discard reshuffled into draw pile.");
    }

    public int RemainingCards()
    {
        return drawPile.Count;
    }

    private void Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            GameObject temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}