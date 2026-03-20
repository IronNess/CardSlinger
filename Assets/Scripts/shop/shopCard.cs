using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class shopCard : MonoBehaviour
{
    [Header("Card details")]
    //public string itemName;       // Name shown in UI!
    public CardEffectType cardType = CardEffectType.None;
    //public GameObject cardPrefab; // The card prefab being sold!
    public int cost = 25;         // Price

    private TextMeshProUGUI displayCost;
    private Button button;

    private shopHandler playerShop;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        displayCost = GetComponentInChildren<TextMeshProUGUI>();
        displayCost.text = "Cost: " + cost.ToString();

        button = GetComponent<Button>();
        button.onClick.AddListener(clicked);

        GameObject playerMoney = GameObject.Find("playerMoney");
        if (playerMoney != null)
        {
            playerShop = playerMoney.GetComponent<shopHandler>();
        }
        else
        {
            Debug.LogError("Player not found in scene");
        }
    }

    private void clicked()
    {
        if (playerShop.playerMoney >= cost)
        {
            Debug.Log("bought");
            playerShop.buyCard(cardType, cost);
            button.onClick.RemoveListener(clicked);
            GetComponent<RawImage>().color = Color.red;
        }
        else
        {
            Debug.Log("not enough");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
