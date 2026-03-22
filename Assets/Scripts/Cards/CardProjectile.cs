using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(XRGrabInteractable))]
public class CardProjectile : MonoBehaviour
{
    [Header("Base Damage")]
    public float damage = 10f;              // Normal damage dealt on hit
    public float minimumThrowSpeed = 1.5f;  // Minimum speed needed to count as a real throw

    [Header("Effect")]
    public CardEffectType effectType = CardEffectType.None; // The special effect this card uses

    [Header("Fire Effect")]
    public float burnDamagePerTick = 2f;    // Damage dealt each burn tick
    public float burnTickInterval = 0.5f;   // Time between each burn tick
    public int burnTicks = 3;               // Number of burn ticks

    [Header("Bounce Effect")]
    public int maxBounces = 3;              // Maximum number of times the card can bounce

    [Header("Duplicate / Scatter")]
    public GameObject cardPrefabOverride;   // Optional prefab used when spawning extra cards
    public int duplicateCount = 1;          // Number of duplicate cards to spawn
    public int scatterCount = 3;            // Number of cards to spawn for scatter
    public float scatterAngle = 15f;        // Angle difference between scatter cards
    public float childVelocityMultiplier = 0.9f; // Extra spawned cards move slightly slower

    [Header("Teleport Effect")]
    public Transform playerRigRoot;         // The XR rig or player root to teleport
    public float teleportYOffset = 1.1176f; //Offset of player heigh so player doesn't teleport into floor or on top of enemy

    [Header("Lifetime")]
    public float lifetimeAfterLanding = 10f; // Time before card despawns after landing
    public bool destroyOnEnemyHit = true;    // Destroy when hitting an enemy
    public bool destroyAfterLifetime = true; // Destroy after landing timer finishes

    private Rigidbody rb;                    // Card rigidbody
    private XRGrabInteractable grabInteractable; // XR grab script

    private bool hasBeenThrown = false;      // True if card was actually thrown
    private bool effectTriggeredOnThrow = false; // Prevents duplicate/scatter from happening multiple times
    private bool hasLanded = false;          // True once the card has hit ground / landed
    private int currentBounceCount = 0;      // How many bounces have happened so far
    private Coroutine despawnRoutine;        // Stores the despawn coroutine

    private void Awake()
    {
        // Get references when the object is created
        rb = GetComponent<Rigidbody>();
        grabInteractable = GetComponent<XRGrabInteractable>();

        if(effectType == CardEffectType.Teleport)
        {
            GameObject player = GameObject.Find("VR Player");
            if (player != null)
            {
                playerRigRoot = player.transform;
            }
            else
            {
                Debug.LogError("Player not found in scene");
            }
        }
    }

    private void OnEnable()
    {
        // Listen for when the player releases or grabs the card
        grabInteractable.selectExited.AddListener(OnReleased);
        grabInteractable.selectEntered.AddListener(OnPickedUp);
    }

    private void OnDisable()
    {
        // Remove listeners to prevent errors
        grabInteractable.selectExited.RemoveListener(OnReleased);
        grabInteractable.selectEntered.RemoveListener(OnPickedUp);
    }

    private void OnPickedUp(SelectEnterEventArgs args)
    {
        // Reset landing and throw states when the player picks the card up
        hasLanded = false;
        hasBeenThrown = false;
        effectTriggeredOnThrow = false;

        // Stop despawn countdown if the player picks it up again
        if (despawnRoutine != null)
        {
            StopCoroutine(despawnRoutine);
            despawnRoutine = null;
        }
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        // Check how fast the card is moving when released
        float speed = rb.linearVelocity.magnitude;

        // Only count as a throw if fast enough
       // hasBeenThrown = speed >= minimumThrowSpeed;

        //if (!hasBeenThrown)
          //  return;

        // Duplicate/scatter effects happen at the moment the card is thrown
        switch (effectType)
        {
            case CardEffectType.Duplicate:
                if (!effectTriggeredOnThrow)
                {
                    effectTriggeredOnThrow = true;
                    SpawnDuplicateCards();
                }
                break;

            case CardEffectType.Scatter:
                if (!effectTriggeredOnThrow)
                {
                    effectTriggeredOnThrow = true;
                    SpawnScatterCards();
                }
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // If the card was not properly thrown, just start the ground timer
        /*if (!hasBeenThrown)
        {
            StartGroundTimer();
            return;
        }*/
        
        // Check if the object hit is an enemy
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                // Deal base damage
                enemyHealth.TakeDamage(damage);

                // Apply special effect depending on card type
                switch (effectType)
                {
                    case CardEffectType.Fire:
                        enemyHealth.ApplyBurn(burnDamagePerTick, burnTickInterval, burnTicks);
                        break;

                    case CardEffectType.Teleport:
                        TeleportPlayerToHitPoint(collision.contacts[0].point);
                        break;

                    case CardEffectType.Bouncy:
                        currentBounceCount++;

                        // If max bounces reached, stop bouncing and start despawn behaviour
                        if (currentBounceCount >= maxBounces)
                        {
                            LandOrDestroy();
                        }
                        return;
                }
            }

            // Destroy card after hitting enemy unless it is a bounce card
            if (destroyOnEnemyHit && effectType != CardEffectType.Bouncy)
            {
                Destroy(gameObject);
                return;
            }
        } 
        else if (collision.gameObject.CompareTag("Environment"))
        {
            // Apply special effect depending on card type
            switch (effectType)
            {
                case CardEffectType.Teleport:
                    TeleportPlayerToHitPoint(collision.contacts[0].point);
                    Destroy(gameObject);
                    break;

                case CardEffectType.Bouncy:
                    currentBounceCount++;

                    // If max bounces reached, stop bouncing and start despawn behaviour
                    if (currentBounceCount >= maxBounces)
                    {
                        LandOrDestroy();
                    }
                    return;
            }
            // If it hit anything else, begin landing/despawn logic
            StartGroundTimer();
        }
    }

    private void StartGroundTimer()
    {
        // Prevent  multiple despawn timers
        if (hasLanded)
            return;

        hasLanded = true;

        if (destroyAfterLifetime)
        {
            if (despawnRoutine != null)
            {
                StopCoroutine(despawnRoutine);
            }

            despawnRoutine = StartCoroutine(DespawnAfterDelay());
        }
    }

    private IEnumerator DespawnAfterDelay()
    {
        // Wait for the set lifetime, then destroy the card
        yield return new WaitForSeconds(lifetimeAfterLanding);
        Destroy(gameObject);
    }

    private void LandOrDestroy()
    {
        // Used by bounce cards once they finish bouncing
        StartGroundTimer();
    }

    private void TeleportPlayerToHitPoint(Vector3 hitPoint)
    {
        // Safety check -  player rig was not assigned
        if (playerRigRoot == null)
        {
            Debug.LogWarning("Teleport card has no playerRigRoot assigned.");
            return;
        }

        // Move player to the enemy hit position with a small Y offset
        Vector3 targetPosition = hitPoint;
        //targetPosition.y += teleportYOffset;
        targetPosition.y = teleportYOffset;
        playerRigRoot.position = targetPosition;
    }

    private void SpawnDuplicateCards()
    {
        // Make sure at least 1 extra card is spawned
        int totalToSpawn = Mathf.Max(duplicateCount, 1);

        GameObject prefabToSpawn = cardPrefabOverride != null ? cardPrefabOverride : gameObject;

        // Spawn duplicate cards with slight angle changes
        for (int i = 0; i < totalToSpawn; i++)
        {
            //SpawnChildCard(i == 0 ? -8f : 8f);
            // Instantiate child in world space so it keeps correct physics
            GameObject child = Instantiate(
                prefabToSpawn,
                transform.position,
                transform.rotation
            );

            // Copy momentum if both have Rigidbodies
            Rigidbody parentRb = GetComponent<Rigidbody>();
            Rigidbody childRb = child.GetComponent<Rigidbody>();

            if (parentRb != null && childRb != null)
            {
                // Match parent's linear and angular velocity
                childRb.linearVelocity = parentRb.linearVelocity;
                childRb.angularVelocity = parentRb.angularVelocity;
            }
            else
            {
                Debug.LogWarning("Either parent or child is missing a Rigidbody. Momentum won't be applied.");
            }
        }
    }

    private void SpawnScatterCards()
    {
        // Make sure at least 2 extra cards are spawned
        int totalToSpawn = Mathf.Max(scatterCount, 2);

        // Calculate start angle so cards spread evenly
        float startAngle = -scatterAngle * (totalToSpawn - 1) * 0.5f;

        GameObject prefabToSpawn = cardPrefabOverride != null ? cardPrefabOverride : gameObject;

        for (int i = 0; i < totalToSpawn; i++)
        {
            float angle = startAngle + (scatterAngle * i);
            GameObject child = Instantiate(
                prefabToSpawn,
                transform.position,
                transform.rotation
            );

            // Copy momentum if both have Rigidbodies
            Rigidbody parentRb = GetComponent<Rigidbody>();
            Rigidbody childRb = child.GetComponent<Rigidbody>();

            if (parentRb != null && childRb != null)
            {
                // Match parent's linear and angular velocity
                Vector3 rotatedVelocity = Quaternion.Euler(0f, angle, 0f) * parentRb.linearVelocity;
                childRb.linearVelocity = rotatedVelocity;
                childRb.angularVelocity = parentRb.angularVelocity;
            }
            else
            {
                Debug.LogWarning("Either parent or child is missing a Rigidbody. Momentum won't be applied.");
            }
            //SpawnChildCard(angle);
        }

        // Destroy original scatter card after splitting
        Destroy(gameObject);
    }

    private void SpawnChildCard(float yAngleOffset)
    {
        // Choose prefab to spawn
        GameObject prefabToSpawn = cardPrefabOverride != null ? cardPrefabOverride : gameObject;

        // Create the extra card slightly in front of the original card
        GameObject clone = Instantiate(
            prefabToSpawn,
            transform.position + transform.forward * 0.15f,
            Quaternion.Euler(0f, yAngleOffset, 0f) * transform.rotation
        );

        CardProjectile childProjectile = clone.GetComponent<CardProjectile>();
        Rigidbody childRb = clone.GetComponent<Rigidbody>();

        if (childProjectile != null)
        {
            // Mark spawned cards as already thrown
            childProjectile.hasBeenThrown = true;
            childProjectile.effectTriggeredOnThrow = true;

            // Prevent loops of respwan cards
            if (childProjectile.effectType == CardEffectType.Duplicate ||
                childProjectile.effectType == CardEffectType.Scatter)
            {
                childProjectile.effectType = CardEffectType.None;
            }
        }

        if (childRb != null)
        {
            // Crd velocity
            //Vector3 rotatedVelocity = Quaternion.Euler(0f, yAngleOffset, 0f) * rb.linearVelocity;
            Vector3 rotatedVelocity = rb.linearVelocity;
            childRb.linearVelocity = rotatedVelocity;// * childVelocityMultiplier;
            childRb.angularVelocity = rb.angularVelocity;
        }
    }
}