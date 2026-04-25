using UnityEngine;
using UnityEngine.AI;

public class shootAtPlayer : MonoBehaviour
{
    private Animator animator;

    public float attackRadius = 8f;
    public float shootInterval = 2f;
    public float projectileSpeed = 2;
    public string targetTag = "Player";

    public GameObject projectilePrefab;
    public Transform ShootPoint;
    [Header("Audio")]
    public AudioClip rangedShootSound;
    public AudioClip footstepSound;
    public float footstepInterval = 0.5f;
    private float footstepTimer = 0f;    
    private AudioSource audioSource;
    private Transform target;
    private NavMeshAgent agent;
    private float nextShotTime;

    private EnemyHealth enemyHealth;

    void Start()
    {
        //new script start
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        enemyHealth = GetComponent<EnemyHealth>();


        GameObject playerObj = GameObject.FindGameObjectWithTag(targetTag);
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("No object with tag 'Player' found for ranged enemy!");
        }

    }

    void Update()
    {
        if (target == null) return;
        float distance = Vector3.Distance(transform.position, target.position);

        //chase
        if (distance > attackRadius)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            bool isMoving = agent.velocity.magnitude > 0.1f;

            //walk animation 
        
            animator.SetBool("IsWalking", isMoving);
            if (isMoving)
            {
                footstepTimer -= Time.deltaTime;

                if (footstepTimer <= 0f)
                {
                    if (footstepSound != null) 
                    audioSource.PlayOneShot(footstepSound);
                    footstepTimer = footstepInterval;
                }
            }
             else
            {
                footstepTimer = 0f;
            }

            return; 
        }
        
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            animator.SetBool("IsWalking", false);
            footstepTimer = 0f;
            Vector3 lookPos = target.position - transform.position;
            lookPos.y = 0; // Prevent tilting
            transform.rotation = Quaternion.LookRotation(lookPos);
            if (Time.time > nextShotTime)
            {
                animator.SetTrigger("Attack");
                Shoot();
                nextShotTime = Time.time + shootInterval;
            }
        
    }

    void Shoot()
    {
        audioSource.PlayOneShot(rangedShootSound);

        //spawn projectivle from shoot point 
        GameObject proj = Instantiate(projectilePrefab, ShootPoint.position, ShootPoint.rotation);


        Rigidbody rb = proj.GetComponent<Rigidbody>();
        rb.linearVelocity = ShootPoint.forward * projectileSpeed;
    }
    public void ApplyStats(EnemyType type)
    {
        
        if (agent != null)
        {
            agent.speed = type.speed;
        }
        if (enemyHealth != null)
        {
        enemyHealth.maxHealth = type.health;
        enemyHealth.currentHealth = type.health;
        }
    }

}
