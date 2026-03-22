using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Scriptable Objects/EnemyType")]
public class EnemyType : ScriptableObject
{
    public string enemyName;
    public GameObject prefab;
    public float speed = 1f;
    public float health = 50f;
    public float damage = 10f;
}
