using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class handles player health and gameover
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private GameOverHandler gameOverHandler;

    // Trigger gameover and disable player upon crashing
    public void Crash()
    {
        gameOverHandler.EndGame();

        gameObject.SetActive(false);
    }
}
