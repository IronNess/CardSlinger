//using System.Numerics;
//using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
public class moveToPlayer : MonoBehaviour
{
    public Transform target;
    public Transform cameraOffset;

    public float speed = 1f;
    public float withinRange = 10f;

    public string targetTag = "Player";
    public string damageTag = "Card";

    public EnemyType type;
    public AudioClip meleeHitSound;

    //private Vector3 targetPos;
    private Animator animator;
    private NavMeshAgent agent;
    private AudioSource audioSource;
    private bool isAttacking = false;
    private float attackCooldown = 1.5f;   // delay between attacks
    private float attackHitDelay = 0.3f;

    //footstep sound
    public AudioClip footstepSound;
    public float footsteppInterval = 0.5f;
    private float footstepTimer = 0f;


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
            agent.stoppingDistance = 0.5f;
        }

    }

    void Update()
    {
        //bool onMesh = NavMesh.SamplePosition(transform.position, out _, 0.1f, NavMesh.AllAreas);
        //Debug.Log("On NavMesh: " + onMesh);
        if (target == null || agent == null) return;
        //targetPos = target.position;
        //targetPos.y += 1;

        float distance = Vector3.Distance(transform.position, target.position);
        if (isAttacking)
        {
            agent.ResetPath();
            animator.SetBool("IsWalking", false);
            return;
        }
        if (distance <= withinRange)
        {
            //nevagent tells the enemy where to go 
            agent.SetDestination(target.position);
            //walk animation
            bool isMoving = agent.velocity.magnitude > 0.1f;
            animator.SetBool("IsWalking", isMoving);
            if (isMoving)
            {
                Vector3 lookPos = target.position;
                lookPos.y = transform.position.y;
                transform.LookAt(lookPos);
            }
            //footsteop sound
            if (isMoving && !isAttacking)
            {
                footstepTimer -= Time.deltaTime;
                if (footstepTimer <= 0f)
                {
                    if (footstepSound != null)
                        audioSource.PlayOneShot(footstepSound);
                    footstepTimer = footsteppInterval;
                }
            }
            else
            {
                footstepTimer = 0f;
            }
        

            //close enough to attack
            if (distance <=0.7f)
            {
                StartCoroutine(AttackRoutine());
            }
        }
        else
        {
            //idle
            animator.SetBool("IsWalking", false);
            agent.ResetPath();
        }
    }
    private System.Collections.IEnumerator AttackRoutine()
    {
        if (isAttacking) yield break;
        isAttacking = true;

        //freeze 
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        //play attack animation
        animator.SetBool("IsWalking", false);
        animator.SetTrigger("Attack");
        //wait
        yield return new WaitForSeconds(attackHitDelay);

        if (target != null)
        {
            HealthSystem playerHealth = target.GetComponentInParent<HealthSystem>();
            if (playerHealth != null)
            {
                audioSource.PlayOneShot(meleeHitSound);
                playerHealth.TakeDamage(type.damage);
            }
        }
        //cooldown
        yield return new WaitForSeconds(attackCooldown);

        //allow for movement again
        agent.isStopped = false;
        isAttacking = false;
    }
   
   
    //called animation event
/*
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name} collided with {other.name}");

        // If enemy reaches player, end the game / stop play mode
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
            Debug.Log("Card hit");
            Destroy(other.gameObject);

            EnemyHealth health = GetComponent<EnemyHealth>();
            if (health != null)
            {
                health.TakeDamage(type.damage);
            }
        }

        // Removed old card-destroy logic.
        // Enemy damage should now be handled by CardProjectile and EnemyHealth 
    }*/

    private void OnTriggerEnter(Collider other)
    {
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
    }

    public void ApplyStats(EnemyType type)
{
    this.type = type;

    EnemyHealth enemyHealth = GetComponent<EnemyHealth>();
    if (enemyHealth != null)
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
}