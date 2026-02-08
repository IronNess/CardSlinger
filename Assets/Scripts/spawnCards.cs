using UnityEngine;

public class spawnCards : MonoBehaviour
{
    public GameObject cardPrefab;
    public float spawnDelay = 3f;

    private float nextSpawnTime;

    void Update()
    {
        if (Time.time > nextSpawnTime)
        {
            SpawnCard();
            nextSpawnTime = Time.time + spawnDelay;
        }
    }

    void SpawnCard()
    {
        GameObject obj = Instantiate(cardPrefab, transform.position, transform.rotation);
    }
}
