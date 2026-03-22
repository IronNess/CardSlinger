using TMPro;
using UnityEngine;

/// <summary>
/// Scene-facing deck manager.
/// This updates UI and forwards deck actions to PersistentDeckState.
/// </summary>
public class DeckManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshPro textCount;

    private PersistentDeckState persistentDeck;

    private void Start()
    {
        persistentDeck = PersistentDeckState.Instance;

        if (persistentDeck == null)
        {
            Debug.LogError("No PersistentDeckState found in scene.");
            return;
        }

        UpdateDeckText();
    }

    public GameObject DrawCard()
    {
        if (persistentDeck == null)
            return null;

        GameObject cardPrefab = persistentDeck.DrawCard();
        UpdateDeckText();
        return cardPrefab;
    }

    public void AddCard(GameObject cardPrefab)
    {
        if (persistentDeck == null)
            return;

        persistentDeck.AddCard(cardPrefab);
        UpdateDeckText();
    }

    public void RemoveCard(GameObject cardPrefab)
    {
        if (persistentDeck == null)
            return;

        persistentDeck.RemoveCard(cardPrefab);
        UpdateDeckText();
    }

    public void DiscardCard(GameObject cardPrefab)
    {
        if (persistentDeck == null)
            return;

        persistentDeck.DiscardCard(cardPrefab);
        UpdateDeckText();
    }

    public void PrepareDeckForNextLevel()
    {
        if (persistentDeck == null)
            return;

        persistentDeck.PrepareDeckForNextLevel();
        UpdateDeckText();
    }

    public void ResetDeckToStartingDeck()
    {
        if (persistentDeck == null)
            return;

        persistentDeck.ResetDeckToStartingDeck();
        UpdateDeckText();
    }

    public int RemainingCards()
    {
        if (persistentDeck == null)
            return 0;

        return persistentDeck.RemainingCards();
    }

    private void UpdateDeckText()
    {
        if (textCount != null && persistentDeck != null)
        {
            textCount.text = persistentDeck.drawPile.Count + "/" +
                             (persistentDeck.drawPile.Count + persistentDeck.discardPile.Count);
        }
    }
}