using System.Collections.Generic;
using UnityEngine;

// A shop item that sells a whole card
[System.Serializable]
public class ShopCardItem
{
    public string itemName;       // Name shown in UI!
    public GameObject cardPrefab; // The card prefab being sold!
    public int cost = 25;         // Price
}

// A shop item that sells an upgrade/effect
[System.Serializable]
public class ShopUpgradeItem
{
    public string itemName;            // Name shown in UI!
    public CardEffectType effectType;  // Effect being sold!
    public int cost = 30;              // Price
}

public class ShopManager : MonoBehaviour
{
    [Header("References")]
    public PlayerCurrency playerCurrency; // Reference to player money script
    public DeckManager deckManager;       // Reference to deck manager

    [Header("Shop Inventory")]
    public List<ShopCardItem> cardItems = new List<ShopCardItem>();         // Cards for sale
    public List<ShopUpgradeItem> upgradeItems = new List<ShopUpgradeItem>(); // Upgrades for sale

    public bool BuyCard(int index)
    {
        // Prevent invalid shop index
        if (index < 0 || index >= cardItems.Count)
            return false;

        ShopCardItem item = cardItems[index];

        // Check if the player can afford it
        if (!playerCurrency.Spend(item.cost))
        {
            Debug.Log("Not enough money to buy card.");
            return false;
        }

        // Add bought card to deck
        deckManager.AddCard(item.cardPrefab);
        Debug.Log($"Bought card: {item.itemName}");
        return true;
    }

    public bool BuyUpgrade(int index, GameObject targetCardPrefab)
    {
        // Prevent invalid index or missing card
        if (index < 0 || index >= upgradeItems.Count || targetCardPrefab == null)
            return false;

        ShopUpgradeItem item = upgradeItems[index];

        // Check if player can afford the upgrade
        if (!playerCurrency.Spend(item.cost))
        {
            Debug.Log("Not enough money to buy upgrade.");
            return false;
        }

        // Get the card script on the target card prefab
        CardProjectile projectile = targetCardPrefab.GetComponent<CardProjectile>();

        if (projectile == null)
        {
            Debug.LogWarning("Target card prefab does not have CardProjectile attached.");
            return false;
        }

        // Change the card's effect type
        projectile.effectType = item.effectType;

        Debug.Log($"Bought upgrade: {item.itemName} for {targetCardPrefab.name}");
        return true;
    }
}