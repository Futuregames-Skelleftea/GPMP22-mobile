using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsListener
{
    // Serialize a boolean variable to toggle test mode in the Unity Ads API
    [SerializeField] private bool testMode = true;

    // Define a static instance of the AdManager class
    public static AdManager Instance;

    // Set the game ID for the Unity Ads API based on the current platform
#if UNITY_ANDROID
    private string gameId = "5243980";
#elif UNITY_IOS
    private string gameId = "5243981";
#endif

    // Declare a GameOverHandler object to handle the game over event
    private GameOverHandler gameOverHandler;

    private void Awake()
    {
        // Check if an instance of the AdManager class already exists
        if (Instance != null && Instance != this)
        {
            // If so, destroy this object
            Destroy(gameObject);
        }
        else
        {
            // Otherwise, set this object as the instance and mark it to persist across scene changes
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Add the AdManager class as a listener for Unity Ads events and initialize the Unity Ads API
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameId, testMode); 
        }
    }

    public void ShowAd(GameOverHandler gameOverHandler)
    {
        // Set the GameOverHandler object to handle the game over event
        this.gameOverHandler = gameOverHandler;

        // Call the Unity Ads API to show a rewarded video ad
        Advertisement.Show("rewardedVideo");
    }
        public void OnUnityAdsDidError(string message)
        {
        // Log an error message if there is an error with Unity Ads
        Debug.LogError($"unity Ads Error: {message}");
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
        // Check the result of the ad display and take appropriate action
        switch (showResult)
            {

                case ShowResult.Finished:
                // If the ad was watched to completion, call the ContinueGame() method of the GameOverHandler object
                gameOverHandler.ContinueGame();
                    break;
                case ShowResult.Skipped:
                    //Ad was skipped
                    break;
                case ShowResult.Failed:
                // If the ad failed to display, log a warning message
                Debug.LogWarning("Ad Failed");
                    break;
            }
        }

        public void OnUnityAdsDidStart(string placementId)
        {
        // Log a message when an ad starts playing
        Debug.Log("Ad Started");
        }

        public void OnUnityAdsReady(string placementId)
        {
        // Log a message when an ad is ready to be displayed
        Debug.Log("Unity Ads Ready");
        }
}