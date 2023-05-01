using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdManager Instance;

	[SerializeField] private bool testMode = true;

	private ScoreWigdetController scoreWigdetController;

	private const string ANDROID_GAME_ID = "5264575";
	private const string IOS_GAME_ID = "5264574";

#if UNITY_ANDROID
	private string gameId = ANDROID_GAME_ID;
#else
	private string gameId = IOS_GAME_ID;
#endif

	private void Awake()
	{

		if (Instance != null && Instance != this)
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

	public void ShowAd(ScoreWigdetController scoreWigdet)
	{
		scoreWigdetController = scoreWigdet;
		Advertisement.Show("Revarded_Video", this);
	}

	public void OnInitializationComplete()
	{
		Debug.Log("Ad initialization complete");
	}

	public void OnInitializationFailed(UnityAdsInitializationError error, string message)
	{
		Debug.Log("Ad initialization failed");
	}

	public void OnUnityAdsAdLoaded(string placementId)
	{
		Debug.Log("Ad loaded");
	}

	public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
	{
		Debug.Log("Ad failed to load");
	}

	public void OnUnityAdsShowClick(string placementId)
	{
		Debug.Log("Ad show click");
	}

	public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
	{
		scoreWigdetController.ResumeGame();
		Debug.Log("Ad show complete");
	}

	public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
	{
		Debug.Log("Ad show failed");
	}

	public void OnUnityAdsShowStart(string placementId)
	{
		Debug.Log("Ad show start");
	}

}
