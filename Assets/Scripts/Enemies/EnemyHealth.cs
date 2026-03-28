using System.Collections;
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

    private Coroutine burnRoutine; // Stores the burn coroutine so can stop/start it safetly

    private Camera mainCamera; // used to always angle healthbar towards the player

    private void Awake()
    {
        healthSlider = healthBar.GetComponent<Slider>();

        currentHealth = maxHealth; // Set enemy's health to full when it spwans in
        healthSlider.maxValue = maxHealth; // update health fill
        healthSlider.value = currentHealth; // update health fill

        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Always face the camera
        healthBar.transform.rotation = Quaternion.LookRotation(healthBar.transform.position - mainCamera.transform.position);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount; // Reduce current health by the damage amount
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthSlider.value = currentHealth; // update health fill
        Debug.Log($"{gameObject.name} took {amount} damage. Health left: {currentHealth}");

        if (currentHealth <= 0f) // If health reaches 0 or below, kill the enemy
        {
            Die();
        }
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

        if (destroyOnDeath) // Destroy the enemy object if enabled
        {
            Destroy(gameObject);
        }
    }
}