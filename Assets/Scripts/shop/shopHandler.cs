using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class shopHandler : MonoBehaviour
{
    [Header("Player")]
    public int playerMoney;
    public TextMeshProUGUI balance;
    public DeckManager deckManager;    
    
    public GameObject fireCardPrefab;
public GameObject teleportCardPrefab;
public GameObject bounceCardPrefab; 
public GameObject duplicateCardPrefab;  // Reference to deck manager

    //[Header("Shop Inventory")]
    //public List<ShopCardItem> cardItems = new List<ShopCardItem>();         // Cards for sale

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        balance.text = "Balance: " + playerMoney.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buyCard(CardEffectType cardType, int cost)
    {
        playerMoney -= cost;
        updateBalance();
        if (deckManager != null)
    {
        GameObject newCard = GetCardPrefab(cardType); // Added so card is added to deck
        deckManager.AddCard(newCard);
    }
    }

    private void updateBalance()
    {
        balance.text = "Balance: " + playerMoney.ToString();
    }

    private GameObject GetCardPrefab(CardEffectType type)
{
    switch (type)
    {
        case CardEffectType.Fire:
            return fireCardPrefab;

        case CardEffectType.Teleport:
            return teleportCardPrefab;

        case CardEffectType.Bounce:
            return bounceCardPrefab;

        case CardEffectType.Duplicate:
        return duplicateCardPrefab;

        default:
            return null;
    }
}
}
