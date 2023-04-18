using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHelper : MonoBehaviour
{
    public void GoToScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void GoToMainMenu()
    {
        GameManager.GoToMainMenu();
    }

    public void GoToPlayScene()
    {
        GameManager.GoToPlayScene();
    }

    public void ContinueGame()
    {
        GameManager.Instance.ContinueGame();
    }

    public void ShowRewardAdContinueGame()
    {
        AdsManager.Instance.ShowRewardContinueGameAd();
    }

    public void QuitGame()
    {
        GameManager.Instance.Quit();
    }
}
