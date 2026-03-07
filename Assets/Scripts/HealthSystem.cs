using UnityEngine;
using UnityEngine.UI;
public class HealthSystem : MonoBehaviour
{
    [Header("Health stats")]
    public float maxHealth = 100.0f;
    private float currentHealth;

    [Header("UI")]
    public Slider healthSlider;
    public Image fillArea;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    //apply damage
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
    }

    //heal
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        TakeDamage(0.1f);
    }

    void UpdateUI()
    {
        healthSlider.value = currentHealth; // updateHealthUI
        if(currentHealth <= 0.0f)
        {
            healthSlider.fillRect.gameObject.SetActive(false);
        }
        else
        {
            healthSlider.fillRect.gameObject.SetActive(true);
        }
    }
}
