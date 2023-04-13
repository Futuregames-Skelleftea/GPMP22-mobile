using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //Load the scene with the build index 1
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
