using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen; 
    [SerializeField] TMP_Text currentScore; 
    [SerializeField] TMP_Text finalScore; 
    [SerializeField] GameObject PausedMenue;
    [SerializeField] GameObject PauseButton;
    [SerializeField] GameObject OptionsMenu;

    public GameObject MoveCanvas;

    public void SetGameOver()
    {
        // Activate the game over screen.
        gameOverScreen.SetActive(true);

        // Set the final score display to the current score.
        finalScore.text = currentScore.text;

        // Deactivate the current score display.
        currentScore.gameObject.SetActive(false);

        // Deactivate the Move Canvas.
        MoveCanvas.gameObject.SetActive(false);

        // Deactivate the Pause Button.
        PauseButton.gameObject.SetActive(false);

        // Stops the music form the adudio manager
        FindObjectOfType<AudioManager>().Stop("Music");

        // Freeze the game by setting the time scale to 0.
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        // Resume the game by setting the time scale to 1.
        Time.timeScale = 1;

        // Load the first scene to restart the game.
        SceneManager.LoadScene(0);
    }

    public void SetPausedMenue()
    {
        // Activates the paused menu game object
        PausedMenue.SetActive(true);

        // Freezes time so that the game is paused
        Time.timeScale = 0;
    }

    public void SetContiniue()
    {
        // Deactivates the paused menu game object
        PausedMenue.SetActive(false);

        // Unfreezes time so that the game continues
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        // Output "Quit" to the console for debugging purposes.
        Debug.Log("Quit");

        // Quit the application.
        Application.Quit();
    }

    public void SetOptionsStart()
    {
        // Activates the options menu game object
        OptionsMenu.SetActive(true);
    }

    public void SetOptionsDone()
    {
        // Deactivates the options menu game object
        OptionsMenu.SetActive(false);
    }
}
