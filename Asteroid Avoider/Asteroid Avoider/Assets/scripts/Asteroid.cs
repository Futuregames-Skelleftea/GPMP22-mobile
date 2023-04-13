using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Get the player health component of the other collider, if it exists.
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        // If the other collider doesn't have a player health component, do nothing.
        if (playerHealth == null)
        {
            return;
        }
        // If the other collider does have a player health component, call its Crash method.
        else
        {
            playerHealth.Crash();
        }
;    }

    // This method is called when this asteroid becomes invisible
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
