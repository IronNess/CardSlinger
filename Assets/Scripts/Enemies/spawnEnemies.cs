using UnityEngine;

public class spawnEnemies : MonoBehaviour
{
    [Header("Player References")]
    public Transform target;
    public Transform cameraOffset;

<<<<<<< HEAD
    [Header("Enemy Setup")]
    public EnemyType[] enemyTypes;
=======
   // public GameObject enemyPrefab;
   public EnemyType[] enemyTypes;
>>>>>>> f8a0d8ebee43e9956c9614425eaca9de60ef4074
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
<<<<<<< HEAD

            float currentDelay = spawnDelay - spawnCount * spawnReduction;

            // Prevent invalid delay values
            if (currentDelay < 0.5f)
                currentDelay = 0.5f;

            nextSpawnTime = Time.time + currentDelay;
=======
            nextSpawnTime = Time.time + spawnDelay - spawnCount * spawnReduction;
>>>>>>> f8a0d8ebee43e9956c9614425eaca9de60ef4074
            spawnCount++;
        }
    }

    void SpawnEnemy()
    {
<<<<<<< HEAD
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
=======
        //enemy type is picked
        EnemyType type = enemyTypes[Random.Range(0, enemyTypes.Length)];

        //spawn point get selected
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        //enemy spawn
>>>>>>> f8a0d8ebee43e9956c9614425eaca9de60ef4074
        GameObject obj = Instantiate(
            type.prefab,
            spawnPoints[spawnIndex].position,
            spawnPoints[spawnIndex].rotation
        );

<<<<<<< HEAD
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
=======
        
       //melee 
        moveToPlayer mover = obj.GetComponent<moveToPlayer>();
		if(mover != null) 
		{		
			mover.setTransforms(target, cameraOffset);
			mover.ApplyStats(type);
    	}
		//range 
		shootAtPlayer shooter = obj.GetComponent<shootAtPlayer>();
		if ( shooter != null) {}
}
}
>>>>>>> f8a0d8ebee43e9956c9614425eaca9de60ef4074
