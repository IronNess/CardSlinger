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

    public EnemyType type;
    public AudioClip meleeHitSound;

    private Vector3 targetPos;
    private Animator animator;
    private NavMeshAgent agent;
    private AudioSource audioSource;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
     void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();


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
        //Debug.Log("On NavMesh: " + onMesh);
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
            //stops the enemy from moving 
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            animator.SetBool("IsWalking", false);

            //deal damage
            HealthSystem playerHealth = other.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                audioSource.PlayOneShot(meleeHitSound);
                playerHealth.TakeDamage(type.damage);
            }
            //atack animation
            animator.SetTrigger("Attack");
        }
        //enemy hit by card
        if (other.CompareTag(damageTag))
        {
            Destroy(other.gameObject);

            EnemyHealth health = GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(type.damage);
            }
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
        this.type = type;
        speed = type.speed;
        if (agent != null)
        {
            agent.speed = speed;
        }
          EnemyHealth health = GetComponent<EnemyHealth>();
    if (health != null)
    {
        health.maxHealth = type.health;
        health.currentHealth = type.health;
    }
    }
}
