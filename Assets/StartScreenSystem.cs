using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScreenSystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI highscoreText;
    [SerializeField] TextMeshProUGUI energyText;
	[SerializeField] int maxEnergy;
	[SerializeField] int energyRechargeTimeInSeconds;

	private int currentEnergy;

	const string CURRENT_ENERGY = "currentenergy";
	const string ENERGY_RECHARGE_KEY = "energyrechargekey";
	const string HIGHSCORE = "highscore";

	private void Start()
	{
		highscoreText.text = $"Current Highscore: {PlayerPrefs.GetInt(HIGHSCORE),0}";
		currentEnergy = PlayerPrefs.GetInt(CURRENT_ENERGY, maxEnergy);
		InitializeRecharge();
	}

	public void StartGame()
	{
		if (currentEnergy > 0)
		{
			currentEnergy--;
			PlayerPrefs.SetInt(CURRENT_ENERGY, currentEnergy);
			if (PlayerPrefs.GetString(ENERGY_RECHARGE_KEY) == string.Empty)
			{
				PlayerPrefs.SetString(ENERGY_RECHARGE_KEY, DateTime.UtcNow.ToString());
			}
			SceneManager.LoadScene(1);
		}
	}

	private void InitializeRecharge()
	{
		string energyRestartTimeString = PlayerPrefs.GetString(ENERGY_RECHARGE_KEY, string.Empty);
		if (PlayerPrefs.GetString(ENERGY_RECHARGE_KEY) != string.Empty)
		{
			if (DateTime.TryParse(energyRestartTimeString, out DateTime rechargeStartedTime))
			{
				var diffInSeconds = (DateTime.UtcNow - rechargeStartedTime).TotalSeconds;
				int addedEnergy = (int)(diffInSeconds / energyRechargeTimeInSeconds);
				currentEnergy += addedEnergy;
				if (currentEnergy >= maxEnergy)
				{
					currentEnergy = maxEnergy;
					PlayerPrefs.SetString(ENERGY_RECHARGE_KEY, string.Empty);
					PlayerPrefs.SetInt(CURRENT_ENERGY, currentEnergy);
				}
				else
				{
					PlayerPrefs.SetInt(CURRENT_ENERGY, currentEnergy);
					int timePassedSinceLastRecharge = (int)diffInSeconds % energyRechargeTimeInSeconds;
					StartCoroutine(EnergyRechargeTimer(energyRechargeTimeInSeconds - timePassedSinceLastRecharge));
				}
			}
		}
		else
		{
			currentEnergy = maxEnergy;
			PlayerPrefs.SetInt(CURRENT_ENERGY, currentEnergy);
		}
		energyText.text = $"Energy: {currentEnergy}/{maxEnergy}";
	}

	private IEnumerator EnergyRechargeTimer(int rechargeTime)
	{
		Debug.Log($"Next energy in {rechargeTime} seconds!");
		yield return new WaitForSeconds(rechargeTime);
		currentEnergy++;
		PlayerPrefs.SetInt(CURRENT_ENERGY, currentEnergy);
		energyText.text = $"Energy: {currentEnergy}/{maxEnergy}";
		if (currentEnergy != maxEnergy)
		{
			PlayerPrefs.SetString(ENERGY_RECHARGE_KEY, DateTime.UtcNow.ToString());
			StartCoroutine(EnergyRechargeTimer(energyRechargeTimeInSeconds));
		}
		else if (currentEnergy == maxEnergy)
		{
			PlayerPrefs.SetString(ENERGY_RECHARGE_KEY, string.Empty);
		}
	}
}
