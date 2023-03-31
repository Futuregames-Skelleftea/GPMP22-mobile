using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    // Boolean flag to enable test mode
    [SerializeField] private bool testMode = true;

    // Game ID for Android and iOS
    public static AdManager Instance;
#if UNITY_ANDROID
    private string gameId = "5227585";
#elif UNITY_IOS
    private string gameId = "5227584";
#endif

    private GameOverHandler gameOverHandler;

    void Awake()
    {
        // If an AdManager instance already exists, destroy this instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // Otherwise, set the instance to this and mark the game object as not destroyable on scene change
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize Unity Ads with the specified game ID and test mode flag
            Advertisement.Initialize(gameId, testMode, this);
        }
    }

    // Method to show a rewarded video ad
    public void ShowAd(GameOverHandler gameOverHandler)
    {
        // Set the game over handler to be called after the ad is watched
        this.gameOverHandler = gameOverHandler;

        // Show a rewarded video ad
        Advertisement.Show("rewardedVideo", this);
    }

    // This method is called when an error occurs during the Unity Ads lifecycle
    public void OnUnityAdsDidError(string message)
    {
        Debug.LogError($"Unity Ads Error: {message}");
    }

    // This method is called when an ad starts playing
    public void OnUnityAdsDidStart(string placementId)
    {
        Debug.Log("Ad Started");
    }

    // This method is called when an ad is loaded and ready to be displayed
    public void OnUnityAdsReady(string placementId)
    {
        Debug.Log("Unity Ads Ready");
    }

    // This method is called when Unity Ads initialization is complete
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        // Load the rewarded video ad after initialization is complete
        Advertisement.Load("rewardedVideo", this);
    }

    // This method is called when Unity Ads initialization fails
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error} - {message}");
    }

    // This method is called when an ad is successfully loaded
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ads Loaded: {placementId}");
    }

    // This method is called when an ad fails to load
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unity {placementId}: {error} - {message}");
    }

    // This method is called when an ad fails to show
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unity {placementId}: {error} - {message}");
    }

    // This method is called when an ad starts showing
    public void OnUnityAdsShowStart(string placementId) { }

    // This method is called when an ad is clicked by the user
    public void OnUnityAdsShowClick(string placementId) { }

    // This method is called when an ad has finished playing and returns information on the completion state
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        switch (showCompletionState)
        {
            // If the ad was successfully watched, call the game over handler
            case UnityAdsShowCompletionState.COMPLETED:
                gameOverHandler.ContinueGame();
                break;

            // If the ad was skipped, do nothing
            case UnityAdsShowCompletionState.SKIPPED:

                // Ad was skipped
                break;

            // If the ad failed to play, log a warning
            case UnityAdsShowCompletionState.UNKNOWN:
                Debug.LogWarning("Ad Failed");
                break;
        }
    }
}
