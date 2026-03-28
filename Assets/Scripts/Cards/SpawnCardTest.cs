using UnityEngine;

public class SpawnCardTest : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform spawnPoint;

    void Update()
    {
        // Press SPACE to spawn a card
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnCard();
        }
    }

    void SpawnCard()
    {
        Instantiate(cardPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}