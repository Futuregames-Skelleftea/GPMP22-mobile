using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float scoreMultiplier;

    public const string HighScoreKey = "Highscore";

    private float score;

    private void Update()
    {
        //update score
        score += Time.deltaTime * scoreMultiplier;

        //update score text.
        scoreText.text = Mathf.FloorToInt(score).ToString(); 
    }

    private void OnDestroy()
    {
        int currentHighScore = PlayerPrefs.GetInt(HighScoreKey, 0);

        //if the current score is higher than the highscore make the current score the new highscore.
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(HighScoreKey, Mathf.FloorToInt(score));
        }
    }

}
