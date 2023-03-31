using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    // Reference to the GameOverHandler object.
    [SerializeField] private GameOverHandler gameOverHandler;

    // This method is called when the player collides with an obstacle.
    public void Crash()
    {
        // Calls the EndGame() method of the GameOverHandler object to trigger the end of the game.
        gameOverHandler.EndGame();

        // Disables the game object to remove it from the scene.
        gameObject.SetActive(false);
    }
}
