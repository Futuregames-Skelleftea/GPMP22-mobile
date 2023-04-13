using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    //variables are set in the inspector
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float scoreMultiplier;

    //storing the state of the score system
    private bool shouldCount = true;
    private float score;

    // Update is called once per frame
    void Update()
    {
        //If shouldCount is false, return and do not update the score
        if(!shouldCount)
        {
            return;
        }

        //Will update score on the amount of time that has passed and scoreMultiplier
        score += Time.deltaTime * scoreMultiplier;

        //Will set scoreText to display the current score
        scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    //Ends timer and stops the score to update
    public int Endtimer()
    {
        //Stoping the score to update
        shouldCount = false;

        //Clearing the scoreText
        scoreText.text = string.Empty;

        //Returning the final score
        return Mathf.FloorToInt(score);
    }

    //Starts the timer
    public void StartTimer()
    {
        //score will update again
        shouldCount = true;
    }
}
