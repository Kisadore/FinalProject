using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Collections;

public class AudioSettings : MonoBehaviour
{

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public AudioMixer masterMixer;
    public AudioMixerGroup musicSource;
    public AudioMixerGroup sfxSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void SetMasterVolume()
    {
        SetVolume("MasterVolume", masterVolumeSlider.value);
    }

    public void SetMusicVolume()
    {
        SetVolume("MusicVolume", musicVolumeSlider.value);
    }

    public void SetSFXVolume()
    {
        SetVolume("SFXVolume", sfxVolumeSlider.value);
    }

    void SetVolume(string parameterName, float sliderValue)
    {
        float adjustedValue = Mathf.Log10(Mathf.Clamp(sliderValue, 0.0001f, 1f)) * 20f;
        if (sliderValue == 0)
            adjustedValue = -80f;

        masterMixer.SetFloat(parameterName, adjustedValue);
    }

    void Update()
    {
        
    }
}
