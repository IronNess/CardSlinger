using UnityEngine;
using UnityEngine.AI;

public class shootAtPlayer : MonoBehaviour
{
    private Animator animator;

    public float attackRadius = 8f;
    public float shootInterval = 2f;
    public float projectileSpeed = 10f;
    public string targetTag = "Player";

    public GameObject projectilePrefab;
    public Transform ShootPoint;
    public AudioClip rangedShootSound;
    private AudioSource audioSource;
    private Transform target;
    private NavMeshAgent agent;
    private float nextShotTime;

    void Start()
    {
        //new script start
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();


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

}