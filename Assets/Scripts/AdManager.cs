using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private bool testMode = true;

    public static AdManager Instance;

#if UNITY_ANDROID
    private string gameId = "5246333";
#elif UNITY_IOS
    private string gameId = "5246332";
#endif

    private GameOverHandler gameOverHandler;

    void Awake()
    {
        if (Instance != null && Instance != this)  // If more than one in scene destroy it.
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            Advertisement.Initialize(gameId, testMode, this);
        }
    }

    public void OnInitializationComplete()  // Ad initialization log.
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)  // Error log.
    {
        Debug.Log($"Unity Ads Initialization Failed: {error} - {message}");
    }

    public void ShowAd(GameOverHandler gameOverHandler)  // Show ad and gameover handler is referenced.
    {
        this.gameOverHandler = gameOverHandler;

        Advertisement.Show("rewardedVideo", this);
    }

    public void OnUnityAdsAdLoaded(string placementId)  // Logs when ad is loaded.
    {
        Debug.Log($"Ad Loaded: {placementId}");
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)  // Error log.
    {
        Debug.Log($"Error loading Ad Unit {placementId}: {error} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)  // Error log.
    {
        Debug.Log($"Error showing Ad Unit {placementId}: {error} - {message}");
    }

    public void OnUnityAdsShowStart(string placementId) { }

    public void OnUnityAdsShowClick(string placementId) { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)  // When Ad is completed.
    {
        switch (showCompletionState)
        {
            case UnityAdsShowCompletionState.COMPLETED: //When Completed, continue game.
                gameOverHandler.ContinueGame();
                break;
            case UnityAdsShowCompletionState.SKIPPED:  // Ad skipped
                
                break;
            case UnityAdsShowCompletionState.UNKNOWN:  //  Error log.
                Debug.LogWarning("Ad Failed");
                break;
        }
    }


}
