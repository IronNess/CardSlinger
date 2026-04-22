using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Scriptable Objects/EnemyType")]
public class EnemyType : ScriptableObject
{
    public string enemyName;
    public GameObject prefab;
    public float speed = 1f;
    public float health = 50f;
    public float damage = 10f;

    [Header("Spawn Settings")]
    public int spawnWeight = 1; // higher = more common 

    // 70+ very common
    // 20+ less cmon
    // 10+ rare 
    // 5+ very rare
    // 1 extremely rare
}
