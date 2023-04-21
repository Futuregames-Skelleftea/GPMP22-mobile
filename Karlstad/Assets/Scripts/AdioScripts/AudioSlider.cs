using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private string audioClipName;
    private string audioVolumeKey = "AudioVolume";

    private void Start()
    {
        // Load audio volume from PlayerPrefs
        float savedVolume = PlayerPrefs.GetFloat(audioVolumeKey, 1f);
        slider.value = savedVolume;

        // Set audio volume to AudioManager
        audioManager.SetVolume(audioClipName, savedVolume);

        // Subscribe to OnValueChanged event
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        // Set audio volume to AudioManager
        audioManager.SetVolume(audioClipName, value);

        PlayerPrefs.SetFloat("AudioVolume", value);

        // Save audio volume to PlayerPrefs
        PlayerPrefs.Save();
    }
}