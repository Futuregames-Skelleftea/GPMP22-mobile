using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //load scene based on buildindex
    public void StartGame(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

}
