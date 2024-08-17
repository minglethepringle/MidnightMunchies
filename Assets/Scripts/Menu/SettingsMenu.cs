using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Slider volumeSlider;
    
    void Start()
    {
        sensitivitySlider.value = PlayerPrefs.GetInt("Sensitivity", 5);
        volumeSlider.value = PlayerPrefs.GetInt("Volume", 10);

        sensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderChanged);
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
    }

    private void OnSensitivitySliderChanged(float value)
    {
        PlayerPrefs.SetInt("Sensitivity", (int)sensitivitySlider.value);
        PlayerLookController.SetMouseSens(sensitivitySlider.value * 40f);
    }

    private void OnVolumeSliderChanged(float value)
    {
        PlayerPrefs.SetInt("Volume", (int)volumeSlider.value);
        PlayerLookController.SetVolume(volumeSlider.value / 10f);
    }
}

