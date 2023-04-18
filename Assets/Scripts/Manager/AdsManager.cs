using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener, IUnityAdsLoadListener
{

    #region Singelton

    /// <summary>
    /// Does the Singelton of Instance Exist
    /// </summary>
    public static bool Exist { get => _instance; }
    /// <summary>
    /// The Refrence to the Instance of GameManager
    /// </summary>
    static private AdsManager _instance;


    /// <summary>
    /// The Public Method to get the Instance of GameManager
    /// </summary>
    public static AdsManager Instance
    {
        get
        {
            // if there are no Instance of GameManager Create one
            if (!Exist) { 
                _instance = Instantiate(Resources.Load<GameObject>("Managers/AdsManager")).GetComponent<AdsManager>(); 
                _instance.InitializeAds();
            }
            // Returns new or existing Instance of GameManager
            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region Interfaces
    [SerializeField]
    private bool _testMode = true;

    private const string _keepPlayingRewardKey = "KeepPlayingReward";

    private string _androidGameID = "5244117";
    private string _iOSGameID = "5244116";
    private string _gameId => (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameID
            : _androidGameID;

    private bool _initilized = false;
    public bool Initilized { get => _initilized; private set => _initilized = value; }

    [ContextMenu("Initilized")]
    private void InitializeAds()
    {
        Advertisement.Initialize(_gameId, _testMode, this);
    }

    //
    // InitilizeAds Interface
    //

    public void OnInitializationComplete()
    {
        Initilized = true;
        LoadAd();

        Debug.Log("Unity Ads initialization Completed");
    }

    // unused inteface methods
    public void OnInitializationFailed(UnityAdsInitializationError error, string message){}
    //

    //
    // ShowAd interface
    //

    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        LoadAd(); // load next add

        // Completed ad

        if (adUnitId.Equals(_keepPlayingRewardKey) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            if (_testMode) Debug.Log($"Unity Ads {_keepPlayingRewardKey} Ad Completed");
            GameManager.Instance.ContinueGame();
        }

        // Skipped ad

        else if (adUnitId.Equals(_keepPlayingRewardKey) && showCompletionState.Equals(UnityAdsShowCompletionState.SKIPPED))
        {
            if (_testMode) Debug.Log($"Unity Ads {_keepPlayingRewardKey} Ad Skipped");
            
            // make ad button interactable again
            GameManager.Instance.CanvasGameOverDisplay.AdButtonInteractable(true);
        }

        // Some Unknown Error happend

        else if (adUnitId.Equals(_keepPlayingRewardKey) && showCompletionState.Equals(UnityAdsShowCompletionState.UNKNOWN))
        {
            if (_testMode) Debug.Log($"Unity Ads {_keepPlayingRewardKey} Ad status");

            // make ad button interactable again
            GameManager.Instance.CanvasGameOverDisplay.AdButtonInteractable(true);
        }
    }

    // unused inteface methods
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) { }
    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }
    //

    //
    // LoadAds interface
    //

    public void LoadAd()
    {
        // Load Reward Ads
        Advertisement.Load(_keepPlayingRewardKey);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (_testMode) Debug.Log($"Loaded ad {placementId}");
        throw new System.NotImplementedException();
    }

    // unused interface methods
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message){}
    //
#endregion

    public void ShowRewardContinueGameAd()
    {
        if (!Initilized) return;
        Advertisement.Show(_keepPlayingRewardKey, this);
    }
}
