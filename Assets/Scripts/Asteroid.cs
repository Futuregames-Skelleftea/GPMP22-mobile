using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public ScoreController scoreController;
    [SerializeField] int scorePerAsteroid;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponentInParent<PlayerMovement>();
            PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>();
            if(playerHealth) playerHealth.GameOver();
        }else if (other.CompareTag("Laser"))
        {
            scoreController.score += scorePerAsteroid;  //adds a bit to the score whenever the player kills an asteroid
            Destroy(other.gameObject);
            Destroy(gameObject);
        }    
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
