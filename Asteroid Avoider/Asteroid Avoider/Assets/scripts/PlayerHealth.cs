using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameOverHandler gameOverHandler;

    // Public method that is called when the player crashes
    public void Crash()
    {
        // Call the EndGame method on the GameOverHandler
        gameOverHandler.EndGame();

        // Deactivate the game object that this script is attached to
        gameObject.SetActive(false);
    }
}
