using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // This method is called when the collider attached to this game object
    private void OnTriggerEnter(Collider other) 
    {
        //Getting the Playerhealth from the other collider's gameobject
        Playerhealth playerHealth = other.GetComponent<Playerhealth>();

        //If other gameobject dont have the Playerhealth component then return
        if(playerHealth == null)
        {
            return;
        }

        //Calling Crash method on Playerhealth script
        playerHealth.Crash();
    }

    //Destroy gameobject when not on screen anymore
    private void OnBecameInvisible() 
    {
        Destroy(gameObject);
    }
}