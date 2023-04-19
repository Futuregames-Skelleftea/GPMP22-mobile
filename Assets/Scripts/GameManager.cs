using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; //the instance to be referenced when accessing the gamemanager
    [SerializeField] TextMeshProUGUI fpsText;   //the fps text element
    [SerializeField] TextMeshProUGUI highScoreText; //the highscore text element
    [SerializeField] float fpsUpdateTime = 0.1f;    //how many times a second the fps text should be updated

    private int saveDataLenght;     //the amount of variables saved
    public float currentCompletedLaps;  //how many laps have been completed this attmept

    //energy variables
    public int energy;
    public int maxEnergy = 5;   //the maximum amount of energy
    public float energyRechargeTime = 1;    //1 minute
    public bool energyRechargeHasBeenAssigned = false;  //if the new recharge time value has been assigned

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
        //sets the energy to whatever it says in the save file
        energy = GetSavedEnergy();

        //sets the instance or destroys the gamemanager if an instance already exists
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

    //loads the main menu scene
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    //function which loads scene by an index
    //kinda useless as other scripts can do this as well by just using the scenemanagement namespace
    //but some special stuff could in theory be added here for some scene transitions
    public void LoadSceneByIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //starts the update fps text coroutine when loading the scene
        if (GameObject.Find("FPS Text") is GameObject tempText)
        {
            if (!fpsText)
            {
                fpsText = tempText.GetComponent<TextMeshProUGUI>();
                StartCoroutine(UpdateFPSText()); 
            }
        }
        
        //if the highscore text is in the newly loaded scene then update the text to whatever is saved
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

    //saving function that saves all the variables based on the variables in the game manager
    //this function is kinda clunky and as such usually best avoided
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

    //Saves a string at a specific index in the save data, much more better compared to the "SaveGame" function
    public void SaveAtIndex(int saveDataIndex, string StringToSave)
    {
        //checks to make sure there is a save file
        if (!Directory.Exists(Application.persistentDataPath)) CreateSaveDirectory();
        if (!File.Exists(Application.persistentDataPath + "/Save.sav")) CreateSaveFile();

        //creates an empty string to write to
        string saveString = "";

        //gets the old save data and splits it up
        string saveData = File.ReadAllText(Application.persistentDataPath + "/Save.sav");   //the entire save data string
        string[] saveDataArray = saveData.Split(";");   //splitting the save data 

        //replaces the save data at the specified index
        saveDataArray[saveDataIndex] = StringToSave;

        //loop which reconstructs the save data with the new information
        for (int i = 0; i < saveDataArray.Length; i++)
        {
            saveString += saveDataArray[i]; 
            //writes a semicolon between all the save variables
            if(i != saveDataArray.Length - 1)
            {
                saveString += ";";
            }
        }

        //writes the new save data to the Save.sav file 
        File.WriteAllText(Application.persistentDataPath + "/Save.sav", saveString);
    }

    //creates a save directory
    private void CreateSaveDirectory()
    {
        Directory.CreateDirectory(Application.persistentDataPath);
        Debug.Log("Creating save path: " + Application.persistentDataPath);
    }

    //creates a save file with placeholder values
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

    //Reads from the save file whether a time has been set for the energy to be recharged at and returns a bool
    public bool GetSavedEnergyReadyTimeHasBeenAssigned()
    {
        //checks to make sure there is a save file
        if (!Directory.Exists(Application.persistentDataPath)) CreateSaveDirectory();
        if (!File.Exists(Application.persistentDataPath + "/Save.sav")) CreateSaveFile();

        string saveData = File.ReadAllText(Application.persistentDataPath + "/Save.sav");   //the entire save data string
        string[] saveDataArray = saveData.Split(";");   //splitting the save data 
        return bool.Parse(saveDataArray[3]);         //whether the recharge time has been set is stored at data position 3
    }
    #endregion
}
