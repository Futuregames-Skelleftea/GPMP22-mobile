using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        // If the other object does not have a PlayerHealth component, return and do nothing   
        if (playerHealth == null) { return; }

        // Call the Crash method of the PlayerHealth component, causing the player to lose health
        playerHealth.Crash();
    }

    // This method is called when the asteroid goes out of view and becomes invisible
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
