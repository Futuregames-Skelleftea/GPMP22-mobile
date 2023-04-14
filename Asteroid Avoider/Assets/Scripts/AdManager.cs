using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;


// This class manages ad display and handles ad events
public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private bool testMode = true;
    public static AdManager Instance;

#if UNITY_ANDROID
    private string gameId = "5245895";
#elif UNITY_IOS
    private string gameId = "5245894";
#endif
    private GameOverHandler gameOverHandler;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Initialize Unity Ads
            Advertisement.Initialize(gameId, testMode, this);
        }
    }

    // Show ad and assign the gameOverHandler instance
    public void ShowAd(GameOverHandler gameOverHandler)
    {
        this.gameOverHandler = gameOverHandler;

        Advertisement.Show("rewardedVideo", this);
    }

    // Ad events
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads Initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("$Unity Ads Initialization failed: {error} - {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ads loaded {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {placementId}: {error} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {placementId}: {error} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId) {    }

    public void OnUnityAdsShowClick(string placementId) {    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        switch(showCompletionState)
        {
            case UnityAdsShowCompletionState.COMPLETED:
            gameOverHandler.ContinueGame();
                break;
            case UnityAdsShowCompletionState.SKIPPED:
                // Ad was skipped
                break;
            case UnityAdsShowCompletionState.UNKNOWN:
            Debug.LogWarning("Ad Failed");
                break;
        }
    }
}