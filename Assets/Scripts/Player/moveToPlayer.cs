using UnityEngine;

public class moveToPlayer : MonoBehaviour
{
    public Transform target;
    public Transform cameraOffset;

    public float speed = 1f;
    public float withinRange = 10f;

    public string targetTag = "Player";

    private Vector3 targetPos;

    void Start()
    {
        // Safety check in case target was not assigned
        if (target == null || cameraOffset == null)
        {
            Debug.LogWarning($"{gameObject.name} is missing target or cameraOffset.");
            return;
        }

        targetPos = target.position;
        targetPos.y = cameraOffset.position.y;

        // Keep enemy on ground height
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
    }

    void Update()
    {
        // Prevent errors if references are missing
        if (target == null)
            return;

        targetPos = target.position;
        targetPos.y += 1;

        float distance = Vector3.Distance(targetPos, transform.position);

        if (distance <= withinRange)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            transform.position = pos;
            transform.LookAt(targetPos);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name} collided with {other.name}");

        // If enemy reaches player, end the game / stop play mode
        if (other.CompareTag(targetTag))
        {
            Debug.Log($"{gameObject.name} collided with player");

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        // Removed old card-destroy logic.
        // Enemy damage should now be handled by CardProjectile and EnemyHealth 
    }

    public void setTransforms(Transform player, Transform cam)
    {
        target = player;
        cameraOffset = cam;
        targetPos = target.position + cameraOffset.position;
    }

    public void ApplyStats(EnemyType type)
{
    speed = type.speed;

    EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
    if (enemyHealth != null)
    {
        enemyHealth.maxHealth = type.health;
        enemyHealth.currentHealth = type.health;
    }
}
}