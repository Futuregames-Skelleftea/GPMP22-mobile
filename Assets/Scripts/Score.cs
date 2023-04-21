using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Score : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] float _scoreMultiplier;

    float _score;

    private void Awake()
    {
        _text.text = "Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        _scoreMultiplier += Time.deltaTime;
    }

    public void UpdateScore()
    {
        _score += _scoreMultiplier;

        _text.text = $"Score: {Mathf.FloorToInt(_score)}";
    }

    public void EndGame()
    {
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);

        if (_score > currentHighScore)
            PlayerPrefs.SetInt("HighScore", Mathf.FloorToInt(_score));

        PlayerPrefs.SetInt("LastScore", Mathf.FloorToInt(_score));

        SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        EndGame();
    }
}
