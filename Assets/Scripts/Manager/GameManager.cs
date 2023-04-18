using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    #region Singelton

    /// <summary>
    /// Does the Singelton of Instance Exist
    /// </summary>
    public static bool Exist { get => _instance; }
    /// <summary>
    /// The Refrence to the Instance of GameManager
    /// </summary>
    static private GameManager _instance;

    /// <summary>
    /// The Public Method to get the Instance of GameManager
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            // if there are no Instance of GameManager Create one
            if (!Exist) _instance = Instantiate(Resources.Load<GameObject>("Managers/GameManager")).GetComponent<GameManager>();
            // Returns new or existing Instance of GameManager
            return _instance;
        }
    }

    private void Awake()
    {
        // make GameManager "global"
        DontDestroyOnLoad(gameObject);

        // Debuging
        if (AdsManager.Instance) Debug.Log("Found Ad Manager");

        // set framerate to 60 if mobile
        if (Application.isMobilePlatform)
        {
            Application.targetFrameRate = 60;
        }
    }

    #endregion

    private PlayerHealth _playerHealth;

    public PlayerHealth PlayerHealth
    {
        get => _playerHealth;
        set => _playerHealth = value;
    }

    private GameOverDisplay _canvasGameOverDisplay;

    public GameOverDisplay CanvasGameOverDisplay
    {
        get => _canvasGameOverDisplay;
        set => _canvasGameOverDisplay = value;
    }

    private ScoreSystem _scoreSystem;

    public ScoreSystem ScoreSystem
    {
        get => _scoreSystem;
        set => _scoreSystem = value;
    }
    public void Crashed()
    {
        // no game over canvas? return
        if (!CanvasGameOverDisplay) return;

        // Activate game over screen
        CanvasGameOverDisplay.gameObject.SetActive(true);

        // Update Score text on game over screen
        CanvasGameOverDisplay.ScoreDisplay.text = $"Your Score: {ScoreSystem.ScoreFloored}";
        
        // stop scoreSystem from adding score
        ScoreSystem.IsPlaying = false;
    }

    public void ContinueGame()
    {
        // activate player health
        PlayerHealth.gameObject.SetActive(true);
        
        // make player invinsible for x seconds
        PlayerHealth.GiveInvincibility();

        // start adding score in score system
        ScoreSystem.IsPlaying = true;
        
        // deactivate game over screen
        CanvasGameOverDisplay.gameObject.SetActive(false);
        
        // set Timescale to normal scale 1
        Time.timeScale = 1f;
    }

    public void Quit()
    {

        Application.Quit();
        Debug.Log("Quit game");
    }

    public void EndGame()
    {
        Time.timeScale = 1f;
    }

    public static void GoToMainMenu()
    {
        // set timescale to normal scale 1
        Time.timeScale = 1f;

        // Load scene MainMenu
        SceneManager.LoadScene(0);
    }

    public static void GoToPlayScene()
    {
        // set timescale to normal scale 1
        Time.timeScale = 1f;

        // Load scene PlayLevel
        SceneManager.LoadScene(1);
    }
}
