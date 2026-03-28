using UnityEngine;

public class spawnEnemies : MonoBehaviour
{
    [Header("Player References")]
    public Transform target;
    public Transform cameraOffset;

    [Header("Enemy Setup")]
    public EnemyType[] enemyTypes;     // Different enemy types this level can spawn
    public Transform[] spawnPoints;    // Spawn positions for this level

    [Header("Spawn Settings")]
    public float spawnDelay = 3f;      
    public float spawnReduction = 0.01f;
    public int totalEnemiesToSpawn = 3; // Total number of enemies this level should spawn

    private float nextSpawnTime;
    private int spawnCount = 0;

    void Update()
    {
        // Stop spawning once the level has spawned enough enemies
        if (spawnCount >= totalEnemiesToSpawn)
            return;

        if (Time.time > nextSpawnTime)
        {
            SpawnEnemy();

            float currentDelay = spawnDelay - spawnCount * spawnReduction;

            // Prevent the delay from becoming too low or negative
            if (currentDelay < 0.5f)
                currentDelay = 0.5f;

            nextSpawnTime = Time.time + currentDelay;
            spawnCount++;
        }
    }

    void SpawnEnemy()
    {
        // Safety check: enemy types must be assigned
        if (enemyTypes == null || enemyTypes.Length == 0)
        {
            Debug.LogWarning("No enemy types assigned on " + gameObject.name);
            return;
        }

        // Safety check: spawn points must be assigned
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned on " + gameObject.name);
            return;
        }

        // Pick a random enemy type from the list
        EnemyType type = enemyTypes[Random.Range(0, enemyTypes.Length)];

        // Pick a random spawn point
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // Spawn the enemy
        GameObject obj = Instantiate(
            type.prefab,
            spawnPoints[spawnIndex].position,
            spawnPoints[spawnIndex].rotation
        );

        // Set up melee / movement enemy
        moveToPlayer mover = obj.GetComponent<moveToPlayer>();
        if (mover != null)
        {
            mover.setTransforms(target, cameraOffset);
            mover.ApplyStats(type);
        }

        // Set up ranged enemy if it has a shoot script
        shootAtPlayer shooter = obj.GetComponent<shootAtPlayer>();
        if (shooter != null)
        {
            // Add ranged setup here later if needed
        }
    }
}