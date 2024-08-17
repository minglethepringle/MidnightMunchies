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
        sensitivitySlider.value = SettingsManager.Instance.Sensitivity;
        volumeSlider.value = SettingsManager.Instance.Volume;

        sensitivitySlider.onValueChanged.AddListener(OnSensitivitySliderChanged);
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
    }

    private void OnSensitivitySliderChanged(float value)
    {
        SettingsManager.Instance.Sensitivity = value;
        PlayerLookController.SetMouseSens(sensitivitySlider.value * 40f);
    }

    private void OnVolumeSliderChanged(float value)
    {
        SettingsManager.Instance.Volume = value;
        PlayerLookController.SetVolume(volumeSlider.value / 10f);
    }
}

