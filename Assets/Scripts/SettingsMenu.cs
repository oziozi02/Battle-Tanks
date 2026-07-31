using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider sfxSlider;
    public Slider musicSlider;
    public GameObject settingsPanel;
    public GameObject playerCountPanel; // reference to hide/show alongside settings

    void OnEnable()
    {
        if (AudioManager.Instance != null)
        {
            sfxSlider.value = AudioManager.Instance.GetSFXVolume();
            musicSlider.value = AudioManager.Instance.GetMusicVolume();
        }
    }

    public void OnSFXSliderChanged(float value)
    {
        AudioManager.Instance.SetSFXVolume(value);
    }

    public void OnMusicSliderChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        if (playerCountPanel != null) playerCountPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        if (playerCountPanel != null) playerCountPanel.SetActive(true);
    }
}