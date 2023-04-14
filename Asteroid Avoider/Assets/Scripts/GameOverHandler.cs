using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.UI;


// This class handles the game over state and its UI interactions
public class GameOverHandler : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private ScoreSystem scoreSystem;
    [SerializeField] private GameObject gameOverDisplay;
    [SerializeField] private AsteroidSpawner asteroidSpawner;

    // End the game, disable asteroid spawning, and display the game over UI
    public void EndGame()
    {
        asteroidSpawner.enabled = false;

        int finalScore = scoreSystem.EndTimer();
        gameOverText.text = $"Your Score: {finalScore}";

        gameOverDisplay.gameObject.SetActive(true);
    }

    // Restart the game by loading the specified scene
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }

    // Show an ad and disable the continue button
    public void ContinueButton()
    {
        AdManager.Instance.ShowAd(this);

        continueButton.interactable = false;
    }

    // Return to the main menu
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    // Continue the game after watching an ad
    internal void ContinueGame()
    {
        scoreSystem.StartTimer(); // q: why i get this error
        player.transform.position = Vector3.zero;
        player.SetActive(true);
        player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;

        asteroidSpawner.enabled = true;

        gameOverDisplay.gameObject.SetActive(false);
    }
}
