using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //This is our first scene
    public void StartGame()
    {
        SceneManager.LoadScene(1); //Loading actual gameplay scene
    }
}
