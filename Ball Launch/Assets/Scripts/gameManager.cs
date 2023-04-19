using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{

    public static gameManager instance;
    int score;
    public Text scoreText;
    public GameObject gameStartUI;



    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Gamestart()
    {
        gameStartUI.SetActive(false);
        scoreText.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ScoreUp()
    {
        score++;
        scoreText.text = score.ToString();
    }
}
