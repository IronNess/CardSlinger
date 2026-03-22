using UnityEngine;

public class shootAtPlayer : MonoBehaviour
{
    public Transform target;
    public GameObject projectilePrefab;
    public float shootInterval = 2f;
    public float projectileSpeed = 0.5f;
    public string targetTag = "Player";


    private float nextShotTime;

    void Start()
    {
    transform.position += transform.forward * 0.2f;

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
        transform.LookAt(target);

        if (Time.time > nextShotTime)
        {
            Shoot();
            nextShotTime = Time.time + shootInterval;
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