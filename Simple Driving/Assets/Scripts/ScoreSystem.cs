using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float scoreMultiplier;

    public const string HighScoreKey = "HighScore";

    private float score;

    void Update()
    {
        score += Time.deltaTime * scoreMultiplier; // Add to score each frame.

        scoreText.text = Mathf.FloorToInt(score).ToString(); // Change the score text and change it to an int.
    }

    private void OnDestroy() 
    {
        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0); // Get high score when destroyed.

        if(score > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, Mathf.FloorToInt(score)); // if current score is higher than current highscore, set the new highscore.

        }
    }
}
