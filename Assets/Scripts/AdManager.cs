using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsLoadListener,IUnityAdsShowListener,IUnityAdsInitializationListener

{
    [SerializeField] bool testMode = true;
    public static AdManager instance;

#if UNITY_ANDROID
    private string gameId = "5245947";
#elif UNITY_IOS
    private string gameId = "5245946";
#endif
    public GameOverHandler gameOverHandler;

   
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            
            Advertisement.Initialize(gameId, false,this);
        }
    }

    public void ShowAd(GameOverHandler gameOverHandler)
    {
        this.gameOverHandler = gameOverHandler;
        Advertisement.Show("rewaredVideo",this); // i miss spelled the ID so this is how it is now
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {

    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {

    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {

    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("showing unity ad, ID: " + placementId.ToString());
    }

    public void OnUnityAdsShowClick(string placementId)
    {

    }

    public void OnInitializationComplete()
    {
        

    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {

    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        switch (showCompletionState)
        {
            case UnityAdsShowCompletionState.COMPLETED:
                Debug.Log("ad completed");
                gameOverHandler.ContinueGame();
                break;
            case UnityAdsShowCompletionState.SKIPPED:
                break;
            case UnityAdsShowCompletionState.UNKNOWN:
                break;
        }
    }
}
