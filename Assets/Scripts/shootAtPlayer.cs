using UnityEngine;
using UnityEngine.AI;

public class shootAtPlayer : MonoBehaviour
{

    public float attackRadius = 8f;
    public float shootInterval = 2f;
    public float projectileSpeed = 10f;
    public string targetTag = "Player";

    public GameObject projectilePrefab;

    private Transform target;
    private NavMeshAgent agent;
    private float nextShotTime;

    void Start()
    {
        //new script start
        agent = GetComponent<NavMeshAgent>();

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
		if(target == null) return;
        float distance = Vector3.Distance(transform.position, target.position);

        //chase
        if (distance > attackRadius)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
        }
        else
        {
            agent.isStopped = true;
            transform.LookAt(target);
            if (Time.time > nextShotTime)
            {
                Shoot();
                nextShotTime = Time.time + shootInterval;
            }
        }
    }

    void Shoot()
    {
        GameObject proj = Instantiate(
            projectilePrefab,
            transform.position + transform.forward * 0.5f + Vector3.up * 0.5f,
            transform.rotation
        );

        proj.GetComponent<Rigidbody>().linearVelocity = transform.forward * projectileSpeed;
    }

}