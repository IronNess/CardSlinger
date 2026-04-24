using UnityEngine;
using UnityEngine.AI;

public class shootAtPlayer : MonoBehaviour
{
    private Animator animator;

    public float attackRadius = 8f;
    public float shootInterval = 2f;
    public float projectileSpeed = 2;
    public string targetTag = "Player";
    public string damageTag = "Card";

    public EnemyType type;

    public GameObject projectilePrefab;
    public Transform ShootPoint;
    public AudioClip rangedShootSound;
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

            //walk animation 
            animator.SetBool("IsWalking", true);
            animator.SetBool("Attack", false);
        }
        else
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.velocity = Vector3.zero;
            animator.SetBool("IsWalking", false);

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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name} collided with {other.name}");

        //enemy hit by card
        if (other.CompareTag(damageTag))
        {
            Debug.Log("Card hit");
            Destroy(other.gameObject);

            EnemyHealth health = GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(other.gameObject.GetComponent<CardProjectile>().damage);
            }
        }

        // Removed old card-destroy logic.
        // Enemy damage should now be handled by CardProjectile and EnemyHealth 
    }
}