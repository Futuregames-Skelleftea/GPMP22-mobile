using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour,  IUnityAdsShowListener, IUnityAdsInitializationListener
{
    //Boolean to enable testmode
    [SerializeField] private bool testMode = true;
    public static AdManager Instance;

    //GameID for Android
#if UNITY_ANDROID
    private string gameID = "5243963";
#endif    

    private GameOverHandler gameOverHandler;    

    private void Awake() 
    {
        //If the instance of AdManager already exists, destroy this one
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        //Set this the instance and dont destroy on load and load advertisement
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            AdManager adManager = this;
            Advertisement.Initialize(gameID, testMode, this);
        }
    }
    //Showing the ad and sets gameoverhandler to be called
    public void ShowAd(GameOverHandler gameOverHandler)
    {
        this.gameOverHandler = gameOverHandler;

        Advertisement.Show("rewardedVideo", this);
    }

    //Implementation of IUnityAdsListener methods that i dont use
    public void OnInitializationComplete(){}
    public void OnInitializationFailed(UnityAdsInitializationError error, string message){}
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message){}
    public void OnUnityAdsShowStart(string placementId){}
    public void OnUnityAdsShowClick(string placementId){}

    //Activated when ad is finished playing
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        switch (showCompletionState)
        {
            //If ad successfully completed, run the gameoverhandler.Continue
            case UnityAdsShowCompletionState.COMPLETED:
                gameOverHandler.Continue();
                break;
            //If ad was skipped
            case UnityAdsShowCompletionState.SKIPPED:
                break;
            //If ad state is unknown log a warning
            case UnityAdsShowCompletionState.UNKNOWN:
                Debug.LogWarning("Ad Failed");
                break;
        }
    }
}
