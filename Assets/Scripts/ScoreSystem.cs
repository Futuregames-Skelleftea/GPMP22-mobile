using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] float _scoreMultiplier;

    public const string HighScoreKey = "HighScore";

    float _score;

    // Increases the player's score the longer they go before crashing
    void Update()
    {
        _score += Time.deltaTime * _scoreMultiplier;

        _scoreText.text = $"Score: {Mathf.FloorToInt(_score).ToString()}";
    }

    // If the player crashes or quits, saves the high score
    private void OnDestroy()
    {
        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        if (_score > currentHighScore)
            PlayerPrefs.SetInt(HighScoreKey, Mathf.FloorToInt(_score));
    }
}
