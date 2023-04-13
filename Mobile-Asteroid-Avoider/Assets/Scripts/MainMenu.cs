using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        //will load scene 1 in index, the Gamelevel
        SceneManager.LoadScene(1);
    }
}
