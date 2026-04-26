using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 30f; // Max health the enemy starts with
    public float currentHealth; // Current health during gameplay

    [Header("Optional")]
    public bool destroyOnDeath = true; // If true, enemy object is destroyed when it dies

    [Header("HealthBar")]
    public GameObject healthBar;
    private Slider healthSlider;

    public AudioClip deathSound;
    private Coroutine burnRoutine; // Stores the burn coroutine so can stop/start it safetly
    private AudioSource audioSource;// audio plays
    private void Awake()
    {
        healthSlider = healthBar.GetComponent<Slider>();

        healthSlider.maxValue = maxHealth;

        currentHealth = maxHealth; // Set enemy's health to full when it spwans in
        audioSource = GetComponent<AudioSource>();
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // Reduce current health by the damage amount
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = currentHealth; // update health fill
        Debug.Log($"{gameObject.name} took {amount} damage. Health left: {currentHealth}");

        StartCoroutine(HitFlash());
        if (currentHealth <= 0f) // If health reaches 0 or below, kill the enemy
        {
            Die();
        }
    }
    //added a hitflash so the player knows the enemy got hit
    private IEnumerator HitFlash()
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();

        foreach (Renderer r in rends)
            r.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        foreach (Renderer r in rends)
            r.material.color = Color.white;
    }

    public void ApplyBurn(float damagePerTick, float tickInterval, int tickCount)
    {
        if (burnRoutine != null) // If enemy is already burning, stop the old burn effect
        {
            StopCoroutine(burnRoutine);
        }

        burnRoutine = StartCoroutine(BurnCoroutine(damagePerTick, tickInterval, tickCount)); // Start a new burn effect
    }

    private IEnumerator BurnCoroutine(float damagePerTick, float tickInterval, int tickCount)
    {
        for (int i = 0; i < tickCount; i++) // Apply burn damage multiple times over time
        {
            TakeDamage(damagePerTick);

            if (currentHealth <= 0f) // Stop burn if enemy dies
            {
                yield break;
            }

            yield return new WaitForSeconds(tickInterval); // Wait before the next burn tick
        }

        burnRoutine = null; // Burn effect finished
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        StartCoroutine(deathFlash());

        if (deathSound != null && audioSource != null)
            audioSource.PlayOneShot(deathSound);

        if (destroyOnDeath) // Destroy the enemy object if enabled
        {
            Destroy(gameObject);
        }
    }
    
    private IEnumerator deathFlash()
    {
        Renderer[] rends = GetComponentsInChildren<Renderer>();

        foreach (Renderer r in rends)
            r.material.color = Color.red;
        yield return new WaitForSeconds(0.15f);
    }
}