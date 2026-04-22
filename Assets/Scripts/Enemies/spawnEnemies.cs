using UnityEditor;
using UnityEngine;

public class spawnEnemies : MonoBehaviour
{
    [Header("Player References")]
    public Transform target;
    public Transform cameraOffset;

    // public GameObject enemyPrefab;
    public EnemyType[] enemyTypes;
    public Transform[] spawnPoints;
    public float spawnDelay = 3f;
    public float spawnReduction = 0.01f;
    public int totalEnemiesToSpawn = 3; // Total number of enemies this level should spawn

    private float nextSpawnTime;
    private int spawnCount = 0;
    public int maxEnemiesAlive = 30;


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
        //enemy type is picked
        EnemyType type = GetWeightedEnemy();

        // Pick a random spawn point
        int spawnIndex = Random.Range(0, spawnPoints.Length);

        // Spawn the enemy
        GameObject obj = Instantiate(
            type.prefab,
            spawnPoints[spawnIndex].position,
            spawnPoints[spawnIndex].rotation
        );

        //melee 
        moveToPlayer mover = obj.GetComponent<moveToPlayer>();
        if (mover != null)
        {
            mover.setTransforms(target, cameraOffset);
            mover.ApplyStats(type);
        }
        //range 
        shootAtPlayer shooter = obj.GetComponent<shootAtPlayer>();
        if (shooter != null) { }
    }

    EnemyType GetWeightedEnemy()
    {
        int totalWeight = 0;

        //sum all weights
        foreach (EnemyType t in enemyTypes)
            totalWeight += t.spawnWeight;

        //pick a random number
        int randomValue = Random.Range(0, totalWeight);

        //find which number enemy falls into
        foreach (EnemyType t in enemyTypes)
        {
            if (randomValue < t.spawnWeight)
                return t;
            randomValue -= t.spawnWeight;
        }
        return enemyTypes[0];//fallback
    }
    int CountEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;

    }
    ///i added this because i am unsure how rounds will work
    /// 
    public void AdjustSpawnWeight(int difficultyLevel)
    {
        foreach (EnemyType t in enemyTypes)
        {
              if (t.enemyName.Contains("Slow"))
                t.spawnWeight = Mathf.Max(5, 80 - difficultyLevel * 5);

            if (t.enemyName.Contains("Fast"))
                t.spawnWeight = 10 + difficultyLevel * 3;

            if (t.enemyName.Contains("Ranged"))
                t.spawnWeight = 5 + difficultyLevel * 2;

            if (t.enemyName.Contains("Tank"))
                t.spawnWeight = Mathf.Max(1, difficultyLevel - 3);
        }
    }
}