using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    public static PlayerCurrency Instance { get; private set; }

    public int currentMoney = 100; // Starting money 
    [SerializeField] private bool persistAcrossScenes = true;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void EnsureInstanceExists()
    {
        if (Instance != null)
            return;

        GameObject currencyObject = new GameObject("PlayerCurrency");
        currencyObject.AddComponent<PlayerCurrency>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        if (persistAcrossScenes)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool CanAfford(int cost)
    {
        // Returns true if player has enough money
        return currentMoney >= cost;
    }

    public bool Spend(int amount)
    {
        // If the player cannot afford the amount nothing happens
        if (!CanAfford(amount))
            return false;

        // Subtract money
        currentMoney -= amount;

        Debug.Log($"Spent {amount}. Money left: {currentMoney}");
        return true;
    }

    public void AddMoney(int amount)
    {
        // Add money to the player's total
        currentMoney += amount;

        Debug.Log($"Gained {amount}. Total money: {currentMoney}");
    }
}