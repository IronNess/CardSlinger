using UnityEngine;

public class Projectile : MonoBehaviour
{
  public string targetTag = "Player";
	void start()
        {
			Destroy(gameObject, 8f);
		}	
	void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.CompareTag(targetTag))
		{
			Debug.Log("Projectile hit player");
			Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; 
#endif
		}
		Destroy(gameObject);
	}
}
