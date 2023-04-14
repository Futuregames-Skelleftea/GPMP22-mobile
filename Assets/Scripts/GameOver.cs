using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] Button _continueButton;
    [SerializeField] TextMeshProUGUI _gameOverText;
    [SerializeField] ScoreSystem _scoreSystem;
    [SerializeField] GameObject _gameOverDisplay;
    [SerializeField] AsteroidSpawner _spawner;

    // Stops the asteroids and displays the game over screen
    public void EndGame()
    {
        _spawner.enabled = false;
        _gameOverText.text = $"Final Score: {_scoreSystem.EndTimer()}";
        _gameOverDisplay.gameObject.SetActive(true);
    }

    // Resumes the game after ad has finished
    public void ContinueGame()
    {
        _scoreSystem.StartTimer();

        _player.SetActive(true);
        _player.transform.position = Vector3.zero;

        _spawner.enabled = true;

        _gameOverDisplay.gameObject.SetActive(false);
    }

    // The three methods below are for the buttons on the game over screen
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }

    public void ContinueButton()
    {
        AdManager.Instance.ShowAd(this);

        _continueButton.interactable = false;
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }


}
