using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    public GameObject projectilePrefab; 
    public Transform firingPoint; 
    public float projectileSpeed = 10f; 
    public float fireInterval = 1f; 
    public float projectileLifetime = 2f; 

    void Start()
    {
        // Invoke the Fire method every fireInterval seconds
        InvokeRepeating("Fire", fireInterval, fireInterval); 
    }

    void Fire()
    {
        // Spawn the projectile at the firing point
        GameObject projectile = Instantiate(projectilePrefab, firingPoint.position, Quaternion.identity);

        // Get the rigidbody component of the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        // Set the velocity of the projectile to the right direction multiplied by the speed
        rb.velocity = transform.right * projectileSpeed;

        // Destroy the projectile after the specified lifetime
        Destroy(projectile, projectileLifetime); 
    }
}
