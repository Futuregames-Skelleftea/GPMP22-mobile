using UnityEngine;

// This class handles asteroid behavior and interactions
public class Asteroid : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {   
            // Get PlayerHealth component from the collided object
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            // Check if PlayerHealth is not null, then trigger crash
            if (playerHealth == null) { return; }

            playerHealth.Crash();
        
    }

    // Destroy the asteroid when it goes offscreen
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
