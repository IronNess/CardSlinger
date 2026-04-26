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
    private PlayerCurrency currency;

    //[Header("Shop Inventory")]
    //public List<ShopCardItem> cardItems = new List<ShopCardItem>();         // Cards for sale

    private void Awake()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        currency = PlayerCurrency.Instance;
        if (currency == null)
        {
            currency = FindObjectOfType<PlayerCurrency>();
        }
        if (currency == null)
        {
            currency = gameObject.AddComponent<PlayerCurrency>();
            currency.currentMoney = playerMoney;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerMoney = currency.currentMoney;
        if (balance != null)
            balance.text = "Balance: " + playerMoney;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void buyCard(CardEffectType cardType, int cost)
    {
        if (!currency.Spend(cost))
        {
            return;
        }

        playerMoney = currency.currentMoney;
        updateBalance();

        GameObject newCard = GetCardPrefab(cardType); // Added so card is added to deck
        if (newCard == null)
            return;

        if (deckManager != null)
        {
            deckManager.AddCard(newCard);
            return;
        }

        if (PersistentDeckState.Instance != null)
        {
            PersistentDeckState.Instance.AddCard(newCard);
        }
    }

    private void updateBalance()
    {
        playerMoney = currency != null ? currency.currentMoney : playerMoney;
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