using UnityEngine;
using UnityEngine.AI;
public class moveToPlayer : MonoBehaviour
{

    public Transform target;
    public Transform cameraOffset;
    //private Vector3 targetPos = new Vector3(0, 0, 0);

    public float speed = 1f;
    public float withinRange = 10f;

    public string targetTag = "Player";
    public string damageTag = "Card";

    private Vector3 targetPos;
    private Animator animator;
    private NavMeshAgent agent;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
    {
        //new script start
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        //GameObject playerObj = GameObject.FindGameObjectWithTag(targetTag);
        if (agent != null)
        {
            //target = playerObj.transform;
            agent.speed = speed;
            agent.stoppingDistance = 1.5f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        bool onMesh = NavMesh.SamplePosition(transform.position, out _, 0.1f, NavMesh.AllAreas);
        Debug.Log("On NavMesh: " + onMesh);
        if (target == null || agent == null) return;
        targetPos = target.position;
        //targetPos.y += 1;

        float distance = Vector3.Distance(targetPos, transform.position);
        
        if (distance <= withinRange)
        {
            //nevagent tells the enemy where to go 
            agent.SetDestination(targetPos);
            //walk animation
            bool isMoving = agent.velocity.magnitude > 0.1f;
            animator.SetBool("IsWalking", isMoving);
            if (isMoving)
            {
                Vector3 lookPos = target.position;
                lookPos.y = transform.position.y;   
                transform.LookAt(lookPos);
            }
        }
        else
        {
            //idle
            animator.SetBool("IsWalking", false);
            agent.ResetPath();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name} collided with {other.name}");
        // Check if the collided object has the target tag
        if (other.CompareTag(targetTag))
        {
            //atack animation
            animator.SetTrigger("Attack");
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

    public void ApplyStats(EnemyType type)
    {
        speed = type.speed;
        if(agent != null)
        {
            agent.speed = speed;
        }
    }
}
