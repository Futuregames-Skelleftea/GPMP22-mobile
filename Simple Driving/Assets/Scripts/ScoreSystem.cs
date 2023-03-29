using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    // Text component to display the score
    [SerializeField] private TMP_Text scoreText;
    // Multiplier to increase the score rate
    [SerializeField] private float scoreMultiplier;

    public const string HighScoreKey = "HighScore";

    private float score;

    // Update is called once per frame
    void Update()
    {
        // Increase the score based on the elapsed time and the score multiplier
        score += Time.deltaTime * scoreMultiplier;

        // Update the score text to display the integer part of the score
        scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    // OnDestroy is called when the object is destroyed or the scene changes
    private void OnDestroy()
    {
        // Get the current high score from the PlayerPrefs
        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        // If the current score is higher than the high score, update the high score
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, Mathf.FloorToInt(score));
        }
    }
}
