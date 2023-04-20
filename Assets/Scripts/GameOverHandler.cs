using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameOverHandler : MonoBehaviour
{
    [SerializeField] TMP_Text gameOverText;
    [SerializeField] ScoreController scoreController;
    [SerializeField] Button continueButton; //the continue button which plays an ad
    [SerializeField] GameObject gameOverCanvasGroup;    //the parent object of the game over canvas elements
    [SerializeField] GameObject player;
    [SerializeField] GameObject laserAnchor; //parent of the laser 
    [SerializeField] AsteroidSpawner asteroidSpawner;

    public void Endgame()
    {
        int finalScore = scoreController.EndScoreIncrease();    //gets the final score
        gameOverText.text = "Your Final Score: " + finalScore.ToString();   //writes the final score

        //disable asteroid spawning
        asteroidSpawner.canSpawn = false;   
        asteroidSpawner.enabled = false;
        gameOverCanvasGroup.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);  //load the game scene
    }

    //button which plays and ad and disables the continue button so you cant play more than one ad
    public void ContinueButton()
    {
        AdManager.instance.ShowAd(this);
        continueButton.interactable = false;
    }


    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    //function which restarts the game after playing an ad
    public void ContinueGame()
    {
        //disable the game over menu
        gameOverCanvasGroup.SetActive(false);

        //start increasing score again
        scoreController.StartTimer();

        //start the asteroid spawner
        asteroidSpawner.enabled = true;
        asteroidSpawner.canSpawn = true;
        asteroidSpawner.StartCoroutine(asteroidSpawner.SpawnAsteroids());

        //reset the players position
        player.transform.position = Vector3.zero;
        player.SetActive(true);
        laserAnchor.transform.position = Vector3.zero;
        laserAnchor.SetActive(true);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //start the shooting coroutine
        player.GetComponent<PlayerMovement>().StartCoroutine(player.GetComponent<PlayerMovement>().LaserShootingCoroutine());
    }
}
