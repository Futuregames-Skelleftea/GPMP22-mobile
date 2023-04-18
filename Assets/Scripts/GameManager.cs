using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;     //the instance of the gamemanager to be referenced when accessed by other scripts
    [SerializeField] TextMeshProUGUI fpsText;   //the text ui element that contains the fps text
    [SerializeField] float fpsUpdateTime = 0.1f;    //how many seconds between each fps timer update
    void Awake()
    {
        //sets the instance 
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

        if (Application.isMobilePlatform) Application.targetFrameRate = 60; //caps the framerate on mobile 
        StartCoroutine(UpdateFPSText());    //coroutine that updates the fps text
    }

    //coroutine that updates the fps text
    IEnumerator UpdateFPSText()
    {
        while (fpsText)
        {
            fpsText.SetText("FPS: " + (1f / Time.unscaledDeltaTime).ToString("0.0"));   //sets the text to whatever the fps is
            yield return new WaitForSecondsRealtime(fpsUpdateTime);
        }
    }

}
