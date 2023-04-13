using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    //variables are set in the inspector
    [SerializeField] private GameObject player;
    [SerializeField] private Button continueButton;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private ScoreSystem scoreSystem;
    [SerializeField] private GameObject gamOverDisplay;
    [SerializeField] private AsteroidSpawner asteroidSpawner;
    public void EndGame()
    {
        //will diable asteroid, stop the scoretimer, show the score and set canvas to true
        asteroidSpawner.enabled = false;

        int finalScore = scoreSystem.Endtimer();
        gameOverText.text = $"Your Score: {finalScore}";

        gamOverDisplay.gameObject.SetActive(true);

    }
    public void PlayAgain()
    {
        //Start a new game
        SceneManager.LoadScene(1);
    }


    public void ContinueButton()
    {
        //will show ad and set the ContinueButton to false
        AdManager.Instance.ShowAd(this);
        continueButton.interactable = false;        
    }
    public void Continue()
    {
        //starting the score timer
        scoreSystem.StartTimer();

        //setting the player center of screen and activate the player again and reset the velocity
        player.transform.position = Vector3.zero;
        player.SetActive(true);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //enable asteroids again
        asteroidSpawner.enabled = true;

        // settinge the canvas to false
        gamOverDisplay.gameObject.SetActive(false);
    }

    public void ReturnToMenu()
    {
        //will load to MainMenu
        SceneManager.LoadScene(0);
    }
}
