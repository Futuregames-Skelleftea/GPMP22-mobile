using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()  // When called load scene 1
    {
        SceneManager.LoadScene(1);
    }
}
