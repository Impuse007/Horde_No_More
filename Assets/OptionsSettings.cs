using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsSettings : MonoBehaviour
{
    public Slider volumeSlider;
    public Slider musicSlider;

    void Start()
    {
        // Initialize sliders with saved values or default values
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);

        // Apply initial values
        AudioListener.volume = volumeSlider.value;
        SFXManager.instance.musicSource.volume = musicSlider.value;

        // Add listeners to handle value changes
        volumeSlider.onValueChanged.AddListener(SetVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }

    public void SetMusicVolume(float value)
    {
        SFXManager.instance.musicSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
}
