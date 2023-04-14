using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsShowListener, IUnityAdsInitializationListener, IUnityAdsLoadListener
{
    [SerializeField] bool _testMode = true;

    public static AdManager Instance;

#if UNITY_ANDROID
    string _gameId = "5246395";
#elif UNITY_IOS
    string _gameId = "5246394";
#endif

    GameOver _gameOverHandler;

    // Creates the singleton
    void Start()
    {
        if(Instance != null && Instance != this)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }

    public void ShowAd(GameOver gameOverHandler)
    {
        this._gameOverHandler = gameOverHandler;
        Advertisement.Show($"rewardedVideo", this);
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Unity Ads Click");
    }

    // Below are the necessary components for the ads interfaces
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        switch (showCompletionState)
        {
            // Resumes the game when ad has finished
            case UnityAdsShowCompletionState.COMPLETED:
                _gameOverHandler.ContinueGame();
                break;
            case UnityAdsShowCompletionState.SKIPPED:
                Debug.Log("Unity Ad Skipped");
                break;
            case UnityAdsShowCompletionState.UNKNOWN:
                Debug.LogWarning("Unkown Ad Error");
                break;
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("Unity Ads Failed");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Unity Ads Started");
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Initialization Complete");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log("Initialization Failed");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"Ad Loaded: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error Loading Ad Unity {placementId}: {error} - {message}");
    }
}
