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
    [SerializeField] Button continueButton;
    [SerializeField] GameObject gameOverCanvasGroup;
    [SerializeField] GameObject player;
    [SerializeField] GameObject laserAnchor;
    [SerializeField] AsteroidSpawner asteroidSpawner;

    public void Endgame()
    {
        int finalScore = scoreController.EndScoreIncrease();
        gameOverText.text = "Your Final Score: " + finalScore.ToString();

        asteroidSpawner.canSpawn = false;
        asteroidSpawner.enabled = false;
        gameOverCanvasGroup.SetActive(true);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
    public void ContinueButton()
    {
        AdManager.instance.ShowAd(this);
        continueButton.interactable = false;
    }
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ContinueGame()
    {
        gameOverCanvasGroup.SetActive(false);

        scoreController.StartTimer();
        asteroidSpawner.enabled = true;
        asteroidSpawner.canSpawn = true;
        asteroidSpawner.StartCoroutine(asteroidSpawner.SpawnAsteroids());

        player.transform.position = Vector3.zero;
        player.SetActive(true);
        laserAnchor.transform.position = Vector3.zero;
        laserAnchor.SetActive(true);

        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<PlayerMovement>().StartCoroutine(player.GetComponent<PlayerMovement>().LaserShootingCoroutine());
    }
}
