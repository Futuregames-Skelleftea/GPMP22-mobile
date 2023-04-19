using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private ScoreScript scoreSystem;
    [SerializeField] private GameObject gameOverDisplay;
    [SerializeField] private Spawner asteroidSpawner;



    //Disable the spawner and enable the game over screen
    public void EndGame()
    {
        asteroidSpawner.enabled = false;

        int finalscore = scoreSystem.StopCount();
        gameOverText.text = $"Your Score : {finalscore}";
        gameOverDisplay.gameObject.SetActive(true);

        


    }
}
