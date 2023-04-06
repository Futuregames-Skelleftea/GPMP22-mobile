using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float scoreMultiplier;

    // a constant string used as the key for storing and retrieving the high score value in PlayerPrefs
    public const string HighScoreKey = "HighScore";
    // the current score
    private float score;


    void Update()
    {
        // increase the score based on the elapsed time since the last frame and the score multiplier
        score += Time.deltaTime * scoreMultiplier;
        // convert the score to an integer and update the text component with the new score
        scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    private void OnDestroy()
    {
        // retrieve the current high score value from PlayerPrefs, defaulting to 0 if there is no stored value
        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        // if the current score is greater than the stored high score
        if (score > currentHighScore)
        {
            // set the high score value in PlayerPrefs to the current score (converted to an integer)
            PlayerPrefs.SetInt(HighScoreKey, Mathf.FloorToInt(score));
        }
    }
}
