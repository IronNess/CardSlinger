using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("Deck")]
    public List<GameObject> drawPile = new List<GameObject>();     // Cards currently available to draw
    public List<GameObject> discardPile = new List<GameObject>();  // Used cards go here

    public bool reshuffleDiscardIntoDraw = true; // If true, discard pile can be reshuffled back in

    public GameObject DrawCard()
    {
        // If there are no cards in the draw pile, reshuffle
        if (drawPile.Count == 0)
        {
            if (reshuffleDiscardIntoDraw && discardPile.Count > 0)
            {
                Reshuffle();
            }
            else
            {
                Debug.LogWarning("No cards left to draw.");
                return null;
            }
        }

        // Take the first card from the draw pile
        GameObject cardPrefab = drawPile[0];
        drawPile.RemoveAt(0);

        return cardPrefab;
    }

    public void AddCard(GameObject cardPrefab)
    {
        // Adds a new card to the draw pile
        drawPile.Add(cardPrefab);
        Debug.Log($"Added card to deck: {cardPrefab.name}");
    }

    public void RemoveCard(GameObject cardPrefab)
    {
        // Remove card from draw pile if present
        if (drawPile.Contains(cardPrefab))
        {
            drawPile.Remove(cardPrefab);
            Debug.Log($"Removed card from draw pile: {cardPrefab.name}");
            return;
        }

        // Remove card from discard pile if present
        if (discardPile.Contains(cardPrefab))
        {
            discardPile.Remove(cardPrefab);
            Debug.Log($"Removed card from discard pile: {cardPrefab.name}");
        }
    }

    public void DiscardCard(GameObject cardPrefab)
    {
        // Places used card into the discard pile
        discardPile.Add(cardPrefab);
    }

    public int RemainingCards()
    {
        // Returns how many cards are left to draw
        return drawPile.Count;
    }

    public void Reshuffle()
    {
        // Put discarded cards back into the draw pile
        drawPile.AddRange(discardPile);
        discardPile.Clear();

        // Shuffle draw pile randomly
        for (int i = 0; i < drawPile.Count; i++)
        {
            GameObject temp = drawPile[i];
            int randomIndex = Random.Range(i, drawPile.Count);
            drawPile[i] = drawPile[randomIndex];
            drawPile[randomIndex] = temp;
        }

        Debug.Log("Discard pile reshuffled into draw pile.");
    }
}