using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public ScoreController scoreController; //controller for the score system
    [SerializeField] int scorePerAsteroid;  //how much each asteroid is worth
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.GetComponentInParent<PlayerMovement>(); //reference to the player movement script
            PlayerHealth playerHealth = other.GetComponentInParent<PlayerHealth>(); //reference to the player health script
            if(playerHealth) playerHealth.GameOver();
        }else if (other.CompareTag("Laser"))    //if the asteroid collides with a laser
        {
            scoreController.score += scorePerAsteroid;  //adds a bit to the score whenever the player kills an asteroid
            Destroy(other.gameObject);  //destroy the laser
            Destroy(gameObject);    //destroy the asteroid
        }    
    }

    //destroy asteroids when no longer visable
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
