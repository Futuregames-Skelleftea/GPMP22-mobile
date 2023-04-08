using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] TextMeshProUGUI fpsText;
    [SerializeField] TextMeshProUGUI highScoreText;

    [SerializeField] float fpsUpdateTime = 0.1f;

    //variables related to saving
    private int saveDataLenght;     //the amount of variables saved
    public float currentCompletedLaps;


    //energy variables

    public int energy;
    public int maxEnergy = 5;
    public float energyRechargeTime = 1;    //1 minute
    public bool energyRechargeHasBeenAssigned = false;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Awake()
    {
        energy = GetSavedEnergy();

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
        StartCoroutine(UpdateFPSText());
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (GameObject.Find("FPS Text") is GameObject tempText)
        {
            if (!fpsText)
            {
                fpsText = tempText.GetComponent<TextMeshProUGUI>();
                StartCoroutine(UpdateFPSText()); 
            }
        }
        
        if(GameObject.Find("HighScore Text") is GameObject tempText2)
        {
            if (!highScoreText)
            {
                highScoreText = tempText2.GetComponent<TextMeshProUGUI>();
                highScoreText.SetText("Completed Laps: " + GetSavedHighscore());
            }
        }

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

    #region Saving

    public void SaveGame()
    {
        //checks to make sure there is a save file
        if (!Directory.Exists(Application.persistentDataPath)) CreateSaveDirectory();
        if (!File.Exists(Application.persistentDataPath + "/Save.sav")) CreateSaveFile();

        //
        //  BELOW ARE THE SAVED VARIABLES
        //
        saveDataLenght = 0; //the amount of saved variables
        string saveString = "";

        //saving highscore level index at data position 0
        if(currentCompletedLaps > GetSavedHighscore()) //only updates if the current highscore is more than the 
        {
            saveString += currentCompletedLaps.ToString() + ";";
            saveDataLenght++;
        }
        else
        {
            saveString += GetSavedHighscore().ToString() + ";";
            saveDataLenght++;
        }

        //saving energy index at data position 1
        saveString += energy.ToString() + ";";
        saveDataLenght++;

        //saving energy ready time at data position 2
        saveString += energyRechargeTime.ToString() + ";"; 
        saveDataLenght++;

        //saving wether the new ready time has been assigned at data position 3
        saveString += energyRechargeHasBeenAssigned.ToString();    //the last position should not have a semicolon at the end, semicolons are only placed between data points
        saveDataLenght++;

        //writes the save data to the Save.sav text file
        File.WriteAllText(Application.persistentDataPath + "/Save.sav", saveString);
    }

    public void SaveAtIndex(int saveDataIndex, string StringToSave)
    {
        //checks to make sure there is a save file
        if (!Directory.Exists(Application.persistentDataPath)) CreateSaveDirectory();
        if (!File.Exists(Application.persistentDataPath + "/Save.sav")) CreateSaveFile();

        string saveString = "";

        string saveData = File.ReadAllText(Application.persistentDataPath + "/Save.sav");   //the entire save data string
        string[] saveDataArray = saveData.Split(";");   //splitting the save data 

        saveDataArray[saveDataIndex] = StringToSave;

        for (int i = 0; i < saveDataArray.Length; i++)
        {
            saveString += saveDataArray[i];
            if(i != saveDataArray.Length - 1)
            {
                saveString += ";";
            }
        }

        File.WriteAllText(Application.persistentDataPath + "/Save.sav", saveString);
    }

    private void CreateSaveDirectory()
    {
        Directory.CreateDirectory(Application.persistentDataPath);
        Debug.Log("Creating save path: " + Application.persistentDataPath);
    }

    private void CreateSaveFile()
    {
        using (StreamWriter sw = File.CreateText(Application.persistentDataPath + "/Save.sav")) ;
        Debug.Log("Creating file: " + Application.persistentDataPath + "/Save.sav");

        //creates a temporary save file
        string tempSaveFile = "0;" + maxEnergy.ToString() + ";" + DateTime.Now.ToString() + ";" + false.ToString();

        Debug.Log(tempSaveFile);
        File.WriteAllText(Application.persistentDataPath + "/Save.sav", tempSaveFile);
    }

    public int GetSavedHighscore()
    {
        //checks to make sure there is a save file
        if (!Directory.Exists(Application.persistentDataPath)) CreateSaveDirectory();
        if (!File.Exists(Application.persistentDataPath + "/Save.sav")) CreateSaveFile();

        string saveData = File.ReadAllText(Application.persistentDataPath + "/Save.sav");   //the entire save data string
        string[] saveDataArray = saveData.Split(";");   //splitting the save data 
        return int.Parse(saveDataArray[0]);         //the highscore is saved on position 0 and is always an integer
    }
    public int GetSavedEnergy()
    {
        //checks to make sure there is a save file
        if (!Directory.Exists(Application.persistentDataPath)) CreateSaveDirectory();
        if (!File.Exists(Application.persistentDataPath + "/Save.sav")) CreateSaveFile();

        string saveData = File.ReadAllText(Application.persistentDataPath + "/Save.sav");   //the entire save data string
        string[] saveDataArray = saveData.Split(";");   //splitting the save data 
        return int.Parse(saveDataArray[1]);         //the energy is saved on position 1 and is always an integer
 
    }
    public DateTime GetSavedEnergyReadyTime()
    {
        //checks to make sure there is a save file
        if (!Directory.Exists(Application.persistentDataPath)) CreateSaveDirectory();
        if (!File.Exists(Application.persistentDataPath + "/Save.sav")) CreateSaveFile();

        string saveData = File.ReadAllText(Application.persistentDataPath + "/Save.sav");   //the entire save data string
        string[] saveDataArray = saveData.Split(";");   //splitting the save data 
        return DateTime.Parse(saveDataArray[2]);         //the energy ready time is saved on position 2 and is always
    }
    public bool GetSavedEnergyReadyTimeHasBeenAssigned()
    {
        //checks to make sure there is a save file
        if (!Directory.Exists(Application.persistentDataPath)) CreateSaveDirectory();
        if (!File.Exists(Application.persistentDataPath + "/Save.sav")) CreateSaveFile();

        string saveData = File.ReadAllText(Application.persistentDataPath + "/Save.sav");   //the entire save data string
        string[] saveDataArray = saveData.Split(";");   //splitting the save data 
        return bool.Parse(saveDataArray[3]);         //wether the recharge time has been assigned is stored at data position 3
    }
    #endregion
}
