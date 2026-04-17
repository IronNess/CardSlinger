using UnityEngine;

public class Projectile : MonoBehaviour
{
	public string targetTag = "Player";
	public float damageAmount = 10f;
	void Start()
	{
		Rigidbody rb = GetComponent<Rigidbody>();
		rb.useGravity = false;
		rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
		Destroy(gameObject, 8f);
	}
	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag(targetTag))
		{
			Debug.Log("Projectile hit player");
			HealthSystem health = collision.collider.GetComponent<HealthSystem>();

			if (health != null)
			{
				health.TakeDamage(damageAmount);
			}
		}

		Destroy(gameObject);
	}
}