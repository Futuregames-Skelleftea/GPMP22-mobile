using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // This method loads the first level of the game when the "Start Game" button is clicked.
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
