using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private ScoreSystem scoreSystem;
    [SerializeField] private GameObject gameOverDisplay;
    [SerializeField] private AsteroidSpawner asteroidSpawner;



    public void EndGame()  // Ends the game
    {
        asteroidSpawner.enabled = false; // Disable the asteroid spawner.

        int finalScore = scoreSystem.EndTimer();  // takes the int from score system.

        gameOverText.text = $"Your Score: {finalScore}";  // changes the game over text to instead show final score.

        gameOverDisplay.gameObject.SetActive(true);  // enables the game over display.

    }

    public void PlayAgain()  // When called load scene 1
    {
        SceneManager.LoadScene(1);
    }

    public void ContinueButton()
    {
        AdManager.Instance.ShowAd(this);

        continueButton.interactable  = false;
    }

    public void ReturnToMenu()  // When called load scene 0
    {
        SceneManager.LoadScene(0);
    }

    public void ContinueGame()
    {
        scoreSystem.StartTimer();  // Start timer again.

        player.transform.position = Vector3.zero;  // Start player in the middle.
        player.SetActive(true);  // Set active again.
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        asteroidSpawner.enabled = true;   // Activate spawnaer again.

        gameOverDisplay.gameObject.SetActive(false);  // Disable the game over display.
    }
}
