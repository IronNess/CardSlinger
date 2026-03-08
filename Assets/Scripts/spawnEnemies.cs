using UnityEngine;

public class spawnEnemies : MonoBehaviour
{
    public Transform target;
    public Transform cameraOffset;

   // public GameObject enemyPrefab;
   public EnemyType[] enemyTypes;
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
        //enemy type is picked
        EnemyType type = enemyTypes[Random.Range(0, enemyTypes.Length)];

        //spawn point get selected
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        //enemy spawn
        GameObject obj = Instantiate(
            type.prefab,
            spawnPoints[spawnIndex].position,
            spawnPoints[spawnIndex].rotation
        );

        
        //pass transforms and stats
        moveToPlayer mover = obj.GetComponent<moveToPlayer>();
        mover.setTransforms(target, cameraOffset);
        mover.ApplyStats(type);

    }
}
