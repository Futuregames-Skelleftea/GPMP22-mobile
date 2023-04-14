using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private ScoreSystem scoreSystem; // Reference to the ScoreSystem script
    [SerializeField] private GameObject gameOverDisplay;
    [SerializeField] private AsteroidSpawner asteroidSpawner; // Reference to the AsteroidSpawner script

    //Called when we crash
    public void EndGame()
    {
        asteroidSpawner.enabled = false; //Disables the AsteroidSpawner

        int finalScore = scoreSystem.EndTimer(); //Get the final score
        gameOverText.text = $"Your Score: {finalScore}"; //Displays our score with a message

        gameOverDisplay.gameObject.SetActive(true); //Activate the message object
    }

    //Called when the the Play Again button is pressed
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }

    //Called when the Return to Menu button is pressed
    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
