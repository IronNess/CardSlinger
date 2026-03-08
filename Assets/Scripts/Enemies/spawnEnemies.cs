using UnityEngine;

public class spawnEnemies : MonoBehaviour
{
    public Transform target;
    public Transform cameraOffset;

    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnDelay = 3f;
    public float spawnReduction = 0.01f;

    private float nextSpawnTime;
    private int spawnCount = 0;

    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnDelay - spawnCount*spawnReduction;
            spawnCount++;
        }
    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        GameObject obj = Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
        obj.GetComponent<moveToPlayer>().setTransforms(target, cameraOffset);
    }
}
