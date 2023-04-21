using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool gameStarted = false; 
    public bool gameOver = false; 
    public GameObject startButton; 
    public TMP_Text scoreText; 
    private float score = 0f; 
    public TMP_Text highScoreText; 
    private int highScore = 0; 
    public GameObject MoveCanvas;
    public GameObject PauseButton;
    public TMP_Text secondHighScoreText;

    void Awake()
    {
        // Pause the game at start
        Time.timeScale = 0;

        // Get the saved high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Display the high score
        highScoreText.text = "High Score: " + highScore;

        // Display the second high score
        secondHighScoreText.text = "High Score: " + highScore;
    }

    void Update()
    {
        // Check if the game has started
        if (gameStarted) 
        {
            // Increase score over time
            score += Time.deltaTime * 2f;

            // Update the score text
            scoreText.text = "Score: " + Mathf.RoundToInt(score);

            // Check if the current score is higher than the high score
            if (score > highScore) 
            {
                // Update the high score
                highScore = Mathf.RoundToInt(score);

                // Update the high score text
                highScoreText.text = "High Score: " + highScore;

                // Update the second high score text
                secondHighScoreText.text = "High Score: " + highScore;

                // Save the new high score
                PlayerPrefs.SetInt("HighScore", highScore); 
            }
        }

        // Check if the player pressed the R key
        if (Input.GetKeyDown(KeyCode.R))
        {
            // Restart the game
            Restart(); 
        }

        // Check if the player pressed the P key
        if (Input.GetKeyDown(KeyCode.P)) 
        {
            // Reset the high score
            ResetHighScore(); 
        }
    }

    void ResetHighScore()
    {
        // Reset the high score
        highScore = 0;

        // Update the high score text
        highScoreText.text = "High Score: " + highScore;

        // Save the new high score
        PlayerPrefs.SetInt("HighScore ", highScore);
    }

    void Restart()
    {
        // Get the current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // Reload the current scene
        SceneManager.LoadScene(currentSceneIndex); 
    }

    public void StartGame()
    {
        // Set the game as started
        gameStarted = true;

        // Resume the game
        Time.timeScale = 1;

        // Hide the start button
        startButton.SetActive(false);

        // Make the score text visible
        scoreText.gameObject.SetActive(true); 

        // Sets the Move Canvas to true
        MoveCanvas.gameObject.SetActive(true);

        // Sets the Pause Button to true
        PauseButton.SetActive(true);

        // Finds the AudioManager and start the music
        FindObjectOfType<AudioManager>().Play("Music");
    }

    public void GameOver()
    {
        // Set the game as over
        gameOver = true; 
    }

}
