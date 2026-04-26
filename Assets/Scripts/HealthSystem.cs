using UnityEngine;
using UnityEngine.UI;
public class HealthSystem : MonoBehaviour
{
    [Header("Health stats")]
    public float maxHealth = 100.0f;
    private float currentHealth;

    [Header("UI")]
    public Slider healthSlider;

    [Header("Invincibility Frames")]
    public float InvincibilityDuration = 2f; //how long player is invincible
    private bool isInvincible = false;

    //audio!
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hitSound;

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
        //ignore damage if invincible
        if (isInvincible)
            return;
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
        //play sound
        if (audioSource != null && hitSound != null) 
        audioSource.PlayOneShot(hitSound);
        //start invinicibility frames
        StartCoroutine(InvincibilityRoutine());
    }

    //heal
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateUI();
    }

    //the invincible frames
        private System.Collections.IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(InvincibilityDuration);
        isInvincible = false;
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
