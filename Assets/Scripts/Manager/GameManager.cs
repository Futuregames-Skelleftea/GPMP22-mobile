using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using System;


#if UNITY_ANDROID
/// Android
using Unity.Notifications.Android;

#elif UNITY_IOS
// IOS
using Unity.Notifications.iOS;

#endif

public class GameManager : MonoBehaviour
{

#region Singelton

    /// <summary>
    /// Does the Singelton of Instance Exist
    /// </summary>
    public static bool Exist { get => _instance; }
    /// <summary>
    /// The Refrence to the Instance of GameManager
    /// </summary>
    static private GameManager _instance;

    /// <summary>
    /// The Public Method to get the Instance of GameManager
    /// </summary>
    public static GameManager Instance 
    {
        get 
        { 
            // if there are no Instance of GameManager Create one
            if (!Exist) _instance = Instantiate(Resources.Load<GameObject>("Managers/GameManager")).GetComponent<GameManager>();
            // Returns new or existing Instance of GameManager
            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

#endregion

    /// <summary>
    /// The Refrence to the UI TextmeshPro that can display the score
    /// </summary>
    private TextMeshProUGUI _scoreText;
    
    /// <summary>
    /// UI Text displaying Score
    /// </summary>
    public TextMeshProUGUI ScoreText
    {
        get => _scoreText;
        set
        {
            value.text = "Score:" + Score.ToString();
            _scoreText = value;
        }
    }

    /// <summary>
    /// the Actual score of the current Game
    /// </summary>
    private int _score = 0;

    /// <summary>
    /// gets score and sets score will automaticly update UI if UI exist
    /// </summary>
    public int Score
    {
        get => _score;
        set
        {
            if (_scoreText) _scoreText.text = "Score:" + value.ToString();
            _score = value;
        }
    }

    /// <summary>
    /// key to find Energy recover Date and Time from PlayerPrefs
    /// </summary>
    private const string _energyRecoverKey = "EnergyRecoverKey";
    
    /// <summary>
    /// Get Date and Time when Energy has reacharched. Will schedule a Notification for when your energy is recovered after setting EnergyRecoverTime
    /// </summary>
    private DateTime EnergyRecoverTime
    {
        get
        {
            // try getting EnergyRecoverTime
            if (DateTime.TryParse(PlayerPrefs.GetString(_energyRecoverKey, ""), out DateTime tempDate))
            {
                // if there is one then send it back
                return tempDate;
            }

            // if none found the Energy is at max and recover time is now
            return DateTime.Now;
        }
        set
        {

            // Schedule a notification for when Energy is full again if recover is not set to now
            if (value != DateTime.Now)
            {
                #if UNITY_ANDROID
                    // Android Notification
                    ScheduleNotification(value);
                #elif UNITY_IOS
                    // IOS Notification
                    ScheduleNotification(0, DateTime.Now.Minute - value.Minute);
                #endif
            }

            // set EnergyRecoverTime
            PlayerPrefs.SetString(_energyRecoverKey,value.ToString());
            // Save it to memory
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// key to find Energy from PlayerPrefs
    /// </summary>
    private const string EnergyKey = "Energy";
    
    /// <summary>
    /// Your max Energy
    /// </summary>
    public const int MaxEnergy = 3;

    /// <summary>
    /// Public Propery to get Energy restores Energy if energy has been recovered should be max
    /// </summary>
    public int Energy
    {
        get
        {
            // if no Energy ever set. Set it to max
            if (!PlayerPrefs.HasKey(EnergyKey))
            {
                Energy = MaxEnergy;
            }

            // get the Energy from memory
            int tempEnergy = PlayerPrefs.GetInt(EnergyKey, MaxEnergy);
            
            // if energy has recovered set Energy to Max
            if (tempEnergy != MaxEnergy)
            {
                if (EnergyRecoverTime < DateTime.Now)
                {
                    tempEnergy = MaxEnergy;
                    Energy = MaxEnergy;
                }
            }

            // return result
            return tempEnergy;
        }

        set
        {
            // set Energy in memory
            PlayerPrefs.SetInt(EnergyKey, value);

            // if energy is one less then max energy set a new Recover time
            if (value == MaxEnergy -1) EnergyRecoverTime = DateTime.Now.AddMinutes(1);
            
            // save the modified temporary memory of PlayerPrefabs to memory
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// True if it GameManager has no Energy
    /// </summary>
    public static bool NoEnergy => Instance.Energy <= 0;

    /// <summary>
    /// key to find Highscore from PlayerPrefs
    /// </summary>
    public const string HighScoreKey = "HighScore";

    /// <summary>
    /// Highscore from playerPrefs
    /// </summary>
    public int HighScore
    {
        get
        {
            // returns score if no score found return 0 (default value)
            return PlayerPrefs.GetInt(HighScoreKey, 0);
        }
        set
        {
            // set highscore in memory
            PlayerPrefs.SetInt(HighScoreKey, value);
            // save the temporary memory to memory of PlayerPrefs
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// The Refrence to the UI TextmeshPro that can display the score
    /// </summary>
    private TextMeshProUGUI _highScoreText;

    /// <summary>
    /// UI Text displaying Score
    /// </summary>
    public TextMeshProUGUI HighScoreText
    {
        get => _highScoreText;
        set
        {
            // When setting TextMesh refrence also set the text
            value.text = $"HighScore: {HighScore.ToString()}";
            // 
            _highScoreText = value;
        }
    }

#if UNITY_ANDROID

    /// <summary>
    /// Id of the Notification Channel
    /// </summary>
    private const string ChannelId = "notification_channel";

    /// <summary>
    /// Schedules a notification for Android
    /// </summary>
    /// <param name="dateTime">Date and Time</param>
    public static void ScheduleNotification(DateTime dateTime)
    {
        // Create a channel for the notification
        AndroidNotificationChannel notificationChannel = new AndroidNotificationChannel
        {
            Id = ChannelId,
            Name = "Notification Channel",
            Description = "Description",
            Importance = Importance.Default,
        };

        // register the notification Channel
        AndroidNotificationCenter.RegisterNotificationChannel(notificationChannel);

        // Create the actuall noticication
        AndroidNotification notification = new AndroidNotification
        {
            Title = "Energy Recharged!",
            Text = "Your energy has recharged, come back to play again!",
            SmallIcon = "default",
            LargeIcon = "default",
            FireTime = dateTime,
        };

        // schedule the notification
        AndroidNotificationCenter.SendNotification(notification, ChannelId);
    }

#elif UNITY_IOS

    /// <summary>
    /// Schedules a notification for IOS
    /// </summary>
    /// <param name="hours">hours until notification Fires</param>
    /// <param name="minuts">minuts until notification Fires</param>
    /// <param name="seconds">seconds until notification Fires</param>
    /// <param name="looping">Will this Notification repeat itself</param>
    public static void ScheduleNotification(int hours = 0, int minuts = 0, int seconds = 0, bool looping = false)
    {
        // Creating Notification
        iOSNotification notification = new iOSNotification
        {
            Title = "Energy Recharged!",
            Subtitle = "Your energy has been recharged",
            Body = "Your energy has recharged, come back to play again!",
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = "category_a",
            ThreadIdentifier = "thread1",
            Trigger = new iOSNotificationTimeIntervalTrigger
            {
                // sets Time of triggering
                TimeInterval = new System.TimeSpan(hours, minuts, seconds), 
                // will it be repeating
                Repeats = looping 
            }
        };

    }

#endif

    /// <summary>
    /// Loads playable level if you have energy
    /// </summary>
    public static void LoadLevel1() 
    {
        if (NoEnergy) return;

        Instance.Energy -= 1;
        Instance.ResetGameVariables();
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Loads Main Menu
    /// </summary>
    public static void LoadMainMenu()
    {

        Instance.ResetGameVariables();
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// saves highscore and loads MainMenu Scene
    /// </summary>
    public static void CarCrashed()
    {
        Instance.SaveHighScore();
        LoadMainMenu();
    }

    /// <summary>
    /// Saves Score if score is higher then current HighScore
    /// </summary>
    private void SaveHighScore()
    {
        if (Score > HighScore)
        {
            HighScore = Score;
        }
    }

    /// <summary>
    /// Reset all temporary variables
    /// </summary>
    public void ResetGameVariables()
    {
        Score = 0;
    }

}