using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreWigdetController : MonoBehaviour
{
    [SerializeField] private float scoreMultiplier;
    [SerializeField] private Canvas gameOverCanvas;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private AsteroidSpawner asteroidSpawner;

	private float playerScore;
	private bool isAlive;

	private void Start()
	{
        isAlive = true;
		gameOverCanvas.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (isAlive)
        {
            UpdatePlayerScore();
        }
	}

    public void WatchAdToContinue()
    {
        AdManager.Instance.ShowAd(this);
    }

    public void ResumeGame()
    {
        gameOverCanvas.gameObject.SetActive(false);
        isAlive = true;
        player.gameObject.SetActive(true);
        player.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, 0);
        if (player.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.velocity = Vector3.zero;
        }
        StartCoroutine(asteroidSpawner.AsteroidSpawnChain());
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

	public void UpdatePlayerScore()
    {
        playerScore += Time.deltaTime * scoreMultiplier;
        scoreText.text = Mathf.RoundToInt(playerScore).ToString();
    }

    public void GameOverState()
    {
        isAlive = false;
        gameOverCanvas.gameObject.SetActive(true);
        gameOverScoreText.text = Mathf.RoundToInt(playerScore).ToString();
    }
}
