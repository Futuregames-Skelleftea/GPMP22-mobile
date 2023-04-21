using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _highScore;
    [SerializeField] TextMeshProUGUI _lastScore;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        _highScore.text = $"High Score: {PlayerPrefs.GetInt("HighScore", 0)}";
        _lastScore.text = $"Last Score: {PlayerPrefs.GetInt("LastScore", 0)}";
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
