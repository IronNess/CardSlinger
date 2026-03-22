using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class shopHandler : MonoBehaviour
{
    [Header("Player")]
    public int playerMoney;
    public TextMeshProUGUI balance;
    public DeckManager deckManager;       // Reference to deck manager

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
    }

    private void updateBalance()
    {
        balance.text = "Balance: " + playerMoney.ToString();
    }
}
