using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _highScoreText;
    [SerializeField] private TextMeshProUGUI _energyText;

    [SerializeField] private int _maxEnergy;
    [SerializeField] private int _energyRechargeMinutes;

    private int _energy;
    private const string EnergyKey = "Energy";
    private const string EnergyReadyKey = "EnergyReady";

    private void Start() {
        OnApplicationFocus(true);
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if(!hasFocus) return;

        CancelInvoke();
        int hiScore = PlayerPrefs.GetInt("hiScore", 0);
        if (hiScore == 0) _highScoreText.gameObject.SetActive(false);
        _highScoreText.text = $"High Score: {hiScore}";
        _energy = PlayerPrefs.GetInt(EnergyKey, _maxEnergy);
        if (_energy <= 0)
        {
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);
            if (energyReadyString == string.Empty) return;
            DateTime energyReady = DateTime.Parse(energyReadyString);
            if (DateTime.Now >= energyReady)
            {
                _energy = _maxEnergy;
                PlayerPrefs.SetInt(EnergyKey, _energy);
                //todo: start countdown for next energy replenish
                //todo: refactor energy replenishment into its own method?
            }
            else
            {
                Invoke(nameof(RechargeEnergy), (energyReady - DateTime.Now).Seconds);
            }
        }

        PlayerPrefs.SetInt(EnergyKey, _energy);

        string energy = _energy.ToString();
        _energyText.text = $"Plays Left: {energy}";
    }

    private void RechargeEnergy()
    {
        _energy = _maxEnergy;
        string energy = _energy.ToString();
        _energyText.text = $"Plays Left: {energy}";
    }

    public void OnPlayPressed()
    {
        //dont play if energy is 0 or less
        if (_energy <= 0) return;
        //decrease and store energy variable
        _energy--;
        PlayerPrefs.SetInt(EnergyKey, _energy);

        //set text to reflect change
        string energy = _energy.ToString();
        _energyText.text = $"Plays Left: {energy}";

        //if energy was just depleted, store new time for energy to recharge
        if (_energy <= 0)
        {
            DateTime energyReady = DateTime.Now.AddMinutes(_energyRechargeMinutes);
            PlayerPrefs.SetString(EnergyReadyKey, energyReady.ToString());

            //(android only) schedule notification for when energy is restored
#if UNITY_ANDROID
            AndroidNotificationHandler androidNotificationHandler = new();
            androidNotificationHandler.ScheduleNotification(energyReady);
#endif
        }

        //load next scene
        SceneManage.LoadNext();
    }



}
