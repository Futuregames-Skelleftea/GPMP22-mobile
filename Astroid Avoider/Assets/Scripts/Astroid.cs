using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
        
        if(playerHealth == null) { return; }

        playerHealth.Crash();
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
