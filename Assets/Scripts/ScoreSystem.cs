using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText; // Displays the score
    [SerializeField] private float scoreMultiplier; // Multiplier to calculate the score

    private bool shouldCount = true; // Whether the score should be counted or not, turn false when whe crashes

    private float score; 

    // Update is called once per frame
    void Update()
    {
        if (!shouldCount)
        {
            return;
        }

        score += Time.deltaTime * scoreMultiplier;  //Increase the score based on the passed time and the score multiplier

        scoreText.text = Mathf.FloorToInt(score).ToString(); //Updates score display
    }

    // Stops counting and returns the final score. Called when the game ends
    public int EndTimer()
    {
        shouldCount = false;

        scoreText.text = string.Empty; //Clears display text

        return Mathf.FloorToInt(score); //Returns our final score and rounds down score float to nearest int
    }
}
