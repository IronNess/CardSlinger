using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    public int currentMoney = 100; // Starting money 

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