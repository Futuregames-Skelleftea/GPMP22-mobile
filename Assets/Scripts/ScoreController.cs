using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;    //text which displays the score
    [SerializeField] private float scoreMultiplier; //how much score should be applied per second

    public float score; //the score 
    public bool stoppedIncreasingScore;
    // Start is called before the first frame update
    void Update()
    {
        if(!stoppedIncreasingScore) IncreaseScore();
    }

    //function which increases the score and updates the score text
    private void IncreaseScore()
    {
        score += Time.deltaTime * scoreMultiplier;
        scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
    }

    //funciton to be called to stop increasing score on game over
    public int EndScoreIncrease()
    {
        stoppedIncreasingScore = true;
        scoreText.text = "";
        return Mathf.FloorToInt(score);
    }

    //function to be called to start increasing score again
    public void StartTimer()
    {
        stoppedIncreasingScore = false;
        
    }
}
