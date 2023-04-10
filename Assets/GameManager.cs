using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
	AndroidNotificationHandler notificationHandler;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		DontDestroyOnLoad(gameObject);
		notificationHandler = GetComponent<AndroidNotificationHandler>();
	}

	private void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
#if UNITY_ANDROID
			int currentEnergy = PlayerPrefs.GetInt("currentenergy", 5);
			if (currentEnergy != 5)
			{
				if (DateTime.TryParse(PlayerPrefs.GetString("energyrechargekey"), out DateTime dateTime))
				{
					var timePassed = (DateTime.UtcNow - dateTime).TotalSeconds;
					int rechargeCDTime = FindObjectOfType<StartScreenSystem>().energyRechargeTimeInSeconds;
					int addedEnergy = (int)(timePassed / rechargeCDTime);
					currentEnergy += addedEnergy;
					if (currentEnergy < 5)
					{
						int notificationAddedSeconds = (5 - currentEnergy) * rechargeCDTime;
						notificationHandler.ScheduleNotification(DateTime.UtcNow.AddSeconds(notificationAddedSeconds));
					}
				}
			}
#endif
		}
	}
}
