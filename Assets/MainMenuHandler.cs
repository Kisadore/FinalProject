// using UnityEngine;
// using UnityEngine.SceneManagement;

// public class MainMenuHandler : MonoBehaviour
// {
//     public void StartGame(){
//         Debug.Log("Starting Game!");
//         SceneManager.LoadScene("GameScene");
//     }

//     public void QuitGame(){
//         Debug.Log("Quitting Game!");
//         Application.Quit();
//     }
// }

// FOR YOUR MAIN MENU SCENE
// This builds on your existing MainMenuHandler

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class MainMenuHandler : MonoBehaviour
{
    [Header("Menu Panels")]
    public GameObject mainMenuPanel; // Your existing panel with buttons
    public GameObject optionsPanel;  // New panel for options
    
    [Header("Music Control")]
    public AudioSource musicSource; // Reference to your music box's audio source
    
    public void StartGame(){
        Debug.Log("Starting Game!");
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame(){
        Debug.Log("Quitting Game!");
        Application.Quit();
    }
    
    // New method for opening options
    public void OpenOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    
    public void BackToMainMenu()
    {
        optionsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}

// SIMPLE OPTIONS CONTROLLER FOR MAIN MENU
// Attach this to your options panel in the main menu

public class SimpleOptionsController : MonoBehaviour
{
    [Header("Volume Sliders")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    
    [Header("Audio Source")]
    public AudioSource menuMusicSource; // Reference to your main menu music
    
    void Start()
    {
        // Load saved volume settings
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        
        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicVolume;
        
        // Apply loaded settings
        SetMasterVolume(masterVolume);
        SetMusicVolume(musicVolume);
    }
    
    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume; // This affects all audio in the game
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    
    public void SetMusicVolume(float volume)
    {
        menuMusicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}

// FOR YOUR GAME SCENE
// Create a pause menu in your game scene

public class GameScenePauseMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject pauseMenuPanel;
    public GameObject optionsPanel;
    
    [Header("Game Music")]
    public AudioSource gameMusicSource; // Reference to your game scene music
    
    private bool isPaused = false;
    
    void Start()
    {
        // Load volume settings when game starts
        LoadVolumeSettings();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }
    
    void LoadVolumeSettings()
    {
        // Apply saved settings to game music
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        
        AudioListener.volume = masterVolume;
        gameMusicSource.volume = musicVolume;
    }
    
    public void Pause()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    
    public void Resume()
    {
        pauseMenuPanel.SetActive(false);
        optionsPanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    
    public void OpenOptions()
    {
        pauseMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }
    
    public void BackToPauseMenu()
    {
        optionsPanel.SetActive(false);
        pauseMenuPanel.SetActive(true);
    }
    
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
    public void QuitGame()
    {
        Debug.Log("Quitting Game!");
        Application.Quit();
    }
}

// GAME SCENE OPTIONS CONTROLLER
// Similar to main menu but for the game scene

public class GameSceneOptionsController : MonoBehaviour
{
    [Header("Volume Sliders")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    
    [Header("Audio Source")]
    public AudioSource gameMusicSource; // Reference to your game music
    
    void OnEnable()
    {
        // Load current volume settings when options open
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        
        masterVolumeSlider.value = masterVolume;
        musicVolumeSlider.value = musicVolume;
    }
    
    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    
    public void SetMusicVolume(float volume)
    {
        gameMusicSource.volume = volume;
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}