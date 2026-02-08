using UnityEngine;

public class CharacterTrigger : MonoBehaviour
{
    public string targetTag = "Enemy";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{gameObject.name} entered trigger with {other.gameObject.name}");
        if (other.CompareTag(targetTag))
        {
            Debug.Log($"{gameObject.name} entered trigger with {other.gameObject.name}");
            // Example: Start dialogue or interaction
        }
    }
}
