using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private ScoreSystem scoreSystem;
    [SerializeField] private GameObject gameOverDisplay;
    [SerializeField] private AsteroidSpawner asteroidSpawner;

    // Called when the game ends
    public void EndGame()
    {
        // Disable the asteroid spawner
        asteroidSpawner.enabled = false;

        // End the game timer and get the final score
        int finalScore = scoreSystem.EndTimer();
        // Set the game over text to display the final score
        gameOverText.text = $"Your Score: {finalScore}";

        // Activate the game over display
        gameOverDisplay.gameObject.SetActive(true);
    }

    // Called when the player wants to play again
    public void PlayAgain()
    {
        // Load the scene with build index 1
        SceneManager.LoadScene(1);
    }

    // Called when the player clicks the continue button
    public void ContinueButton()
    {
        // Show an ad using the AdManager script
        AdManager.Instance.ShowAd(this);

        // Disable the continue button
        continueButton.interactable = false;
    }

    // Called when the player wants to return to the main menu
    public void ReturnToMenu()
    {
        // Load the scene with build index 0
        SceneManager.LoadScene(0);
    }

    // Called when the player chooses to continue playing after watching an ad
    public void ContinueGame()
    {
        // Start the game timer
        scoreSystem.StartTimer();

        // Set the player position to the center of the screen
        player.transform.position = Vector3.zero;
        // Activate the player GameObject
        player.SetActive(true);

        // Enable the asteroid spawner
        asteroidSpawner.enabled = true;

        // Deactivate the game over display
        gameOverDisplay.gameObject.SetActive(false);
    }
}
