using UnityEngine;

public class PlayerCollideManager : MonoBehaviour
{
    [SerializeField] GameOverManager gameOverManager;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object we collided with has the "Enemy" tag.
        if (collision.gameObject.CompareTag("Enemy"))
        {
            FindObjectOfType<AudioManager>().Play("Explotion");

            // If it does, print "Game Over" to the console.
            Debug.Log("Game Over");

            // Call the "SetGameOver" function on the GameOverManager object.
            gameOverManager.SetGameOver();

            // Destroy the object that this script is attached to.
            Destroy(gameObject);
        }
    }
}

