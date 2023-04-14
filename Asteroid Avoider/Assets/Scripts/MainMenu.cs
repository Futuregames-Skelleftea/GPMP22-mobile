using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This class handles the main menu interactions
public class MainMenu : MonoBehaviour
{
    // Start the game
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
