using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    // Reference to the TextMeshPro Text component where the score is displayed.
    [SerializeField] private TMP_Text scoreText;

    // A multiplier to control how fast the score increases.
    [SerializeField] private float scoreMultiplier;

    // A constant string used as the key for storing high scores in PlayerPrefs.
    public const string HighScoreKey = "HighScore";

    // The current score value.
    private float score;

    // Update is called once per frame.
    void Update()
    {
        // Increase the score based on time and the score multiplier.
        score += Time.deltaTime * scoreMultiplier;

        // Update the score text with the current integer value of the score.
        scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    // Called when the GameObject this script is attached to is destroyed.
    private void OnDestroy()
    {
        // Retrieve the current high score from PlayerPrefs using the HighScoreKey.
        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        // Compare the current score with the stored high score.
        if (score > currentHighScore)
        {
            // If the current score is higher, update the high score in PlayerPrefs.
            PlayerPrefs.SetInt(HighScoreKey, Mathf.FloorToInt(score));
        }
    }
}
