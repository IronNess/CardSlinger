using UnityEngine;

public class moveToPlayer : MonoBehaviour
{

    public Transform target;
    public Transform cameraOffset;
    private Vector3 targetPos = new Vector3(0, 0, 0);

    public float speed = 1f;
    public float withinRange = 10f;

    public string targetTag = "Player";
    public string damageTag = "Card";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        targetPos = target.position;
        targetPos.y = cameraOffset.position.y;
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
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
        // Check if the collided object has the target tag
        if (other.CompareTag(targetTag))
        {
            Debug.Log($"{gameObject.name} collided with {other.name}");
#if UNITY_EDITOR
            // Stop play mode in the Unity Editor
            UnityEditor.EditorApplication.isPlaying = false;

#else
        // Quit the application in a build
        Application.Quit();
#endif
            // Example: Stop movement or trigger animation
        }
        // Check if the collided object has the target tag
        if (other.CompareTag(damageTag))
        {
            Debug.Log($"{gameObject.name} collided with {other.name}");
            Destroy(other.gameObject);
            Destroy(gameObject);
            // Example: Stop movement or trigger animation
        }
    }

    public void setTransforms(Transform player, Transform cam)
    {
        target = player;
        cameraOffset = cam;
        targetPos = target.position + cameraOffset.position;
    }
}
