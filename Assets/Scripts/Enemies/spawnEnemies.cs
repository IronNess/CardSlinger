using UnityEngine;

public class spawnEnemies : MonoBehaviour
{
    [Header("Player References")]
    public Transform target;
    public Transform cameraOffset;

    [Header("Enemy Setup")]
    public EnemyType[] enemyTypes;
    public Transform[] spawnPoints;

    [Header("Spawn Settings")]
    public float spawnDelay = 3f;
    public float spawnReduction = 0.01f;
    public int totalEnemiesToSpawn = 3;

    private float nextSpawnTime;
    private int spawnCount = 0;

    void Update()
    {
        // Stop spawning after this level has spawned enough enemies
        if (spawnCount >= totalEnemiesToSpawn)
            return;

        if (Time.time > nextSpawnTime)
        {
            SpawnEnemy();

            float currentDelay = spawnDelay - spawnCount * spawnReduction;

            // Prevent invalid delay values
            if (currentDelay < 0.5f)
                currentDelay = 0.5f;

            nextSpawnTime = Time.time + currentDelay;
            spawnCount++;
        }
    }

    void SpawnEnemy()
    {
        if (enemyTypes == null || enemyTypes.Length == 0)
        {
            Debug.LogWarning("No enemy types assigned on " + gameObject.name);
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("No spawn points assigned on " + gameObject.name);
            return;
        }

        // Pick a random enemy type
        EnemyType type = enemyTypes[Random.Range(0, enemyTypes.Length)];

        // Pick a random spawn point
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // Spawn enemy
        GameObject obj = Instantiate(
            type.prefab,
            spawnPoints[spawnIndex].position,
            spawnPoints[spawnIndex].rotation
        );

        // Melee setup
        moveToPlayer mover = obj.GetComponent<moveToPlayer>();
        if (mover != null)
        {
            mover.setTransforms(target, cameraOffset);
            mover.ApplyStats(type);
        }

        // Ranged setup
        shootAtPlayer shooter = obj.GetComponent<shootAtPlayer>();
        if (shooter != null)
        {
            // Only use these if they exist in your shootAtPlayer script
            // shooter.SetTarget(target, cameraOffset);
            // shooter.ApplyStats(type);
        }
    }
}