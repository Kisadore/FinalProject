using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System;


public class ScreenSettings : MonoBehaviour
{

    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle;

    void Start()
    {
        resolutions = Screen.resolutions;
        for (int i = 0; i < resolutions.Length; i++)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(resolutions[i].ToString()));
        }

        
        Resolution currentResolution = Screen.currentResolution;

        int currentIndex = PlayerPrefs.GetInt("resolution", -1);
        if(currentIndex == -1){
            currentIndex = Array.IndexOf(resolutions, currentResolution);
        }

        resolutionDropdown.value = currentIndex;

        bool isFullscreen = PlayerPrefs.GetInt("fullscreen", Screen.fullScreen ? 1 : 0) == 1;
        Screen.fullScreen = isFullscreen;
        fullscreenToggle.isOn = isFullscreen;

        vsyncToggle.isOn = (QualitySettings.vSyncCount == 1);
        QualitySettings.vSyncCount = PlayerPrefs.GetInt("vsync", 1);
    }

    public void SetResolution(){
        int currentIndex = resolutionDropdown.value;
        Resolution rez = resolutions[currentIndex];
        Screen.SetResolution(rez.width, rez.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolution", currentIndex);
    }
    public void SetVSync(bool isOn){
        QualitySettings.vSyncCount = isOn ? 1 : 0;
        PlayerPrefs.SetInt("vsync", isOn ? 1 : 0);
    }

    public void SetFullscreen(bool isFullScreen){
        Screen.fullScreen = isFullScreen;
        PlayerPrefs.SetInt("fullscreen", isFullScreen ? 1 : 0);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
