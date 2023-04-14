using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Starts the game
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
