using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    // Reference to player GameObject
    [SerializeField] private GameObject player;

    // Reference to continue button
    [SerializeField] private Button continueButton;

    // Reference to game over text
    [SerializeField] private TMP_Text gameOverText;

    // Reference to score system
    [SerializeField] private ScoreSystem scoreSystem;

    // Reference to game over display
    [SerializeField] private GameObject gameOverDisplay;

    // Reference to asteroid spawner
    [SerializeField] private AsteroidSpawner asteroidSpawner;

    // Method to handle the end of the game
    public void EndGame()
    {
        // Disable the asteroid spawner
        asteroidSpawner.enabled = false;

        // Get the final score
        int finalScore = scoreSystem.EndTimer();

        // Update the game over text with the final score
        gameOverText.text = $"You Score: {finalScore}";

        // Show the game over display
        gameOverDisplay.gameObject.SetActive(true);
    }

    // Method to handle playing the game again
    public void PlayAgain()
    {
        // Load the scene again
        SceneManager.LoadScene(1);
    }

    // Method to handle the continue button
    public void ContinueButton()
    {
        // Show an ad
        AdManager.Instance.ShowAd(this);

        // Disable the continue button
        continueButton.interactable = false;
    }

    // Method to return to the main menu
    public void ReturnToMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene(0);
    }

    // Method to continue the game after watching an ad
    public void ContinueGame()
    {
        // Start the score timer
        scoreSystem.StartTimer();

        // Reset the player position
        player.transform.position = Vector3.zero;

        // Make the player game object active
        player.SetActive(true);

        // Reset the player velocity
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Enable the asteroid spawner
        asteroidSpawner.enabled = true;

        // Hide the game over display
        gameOverDisplay.gameObject.SetActive(false);
    }

}
