using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseMenuPanel;
    public Button resumeButton;
    public Button quitButton;
    public TextMeshProUGUI pauseTitle;
    
    [Header("Main Menu Scene")]
    public string mainMenuSceneName = "MainMenu"; 
    private bool isPaused = false;
    
    void Start()
    {
        Debug.Log("PauseMenu: Initializing...");
        
        // Hide pause menu initially
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        else
        {
            Debug.LogError("PauseMenu: pauseMenuPanel is not assigned!");
        }

        if (resumeButton != null)
        {
            resumeButton.onClick.RemoveAllListeners();
            resumeButton.onClick.AddListener(OnResumeClicked);
            Debug.Log("PauseMenu: Resume button listener added");
        }
        else
        {
            Debug.LogError("PauseMenu: resumeButton is not assigned!");
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(OnQuitClicked);
            Debug.Log("PauseMenu: Quit button listener added");
        }
        else
        {
            Debug.LogError("PauseMenu: quitButton is not assigned!");
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        
    }
    

    public void OnResumeClicked()
    {
        Debug.Log("Resume button clicked!");
        ResumeGame();
    }
    
    public void OnQuitClicked()
    {
        Debug.Log("Quit button clicked!");
        QuitToMainMenu();
    }
    
    public void PauseGame()
    {

        if (GameManager.Instance != null && !GameManager.Instance.IsGameActive())
        {
            Debug.Log("PauseMenu: Cannot pause, game is not active");
            return;
        }
        
        isPaused = true;
        Time.timeScale = 0f;
        // Show pause menu
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
        
        // Show cursor for menu interaction
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        // Disable player input
        var playerInput = FindObjectOfType<PlayerInputHandler>();
        if (playerInput != null)
        {
            playerInput.enabled = false;
        }
        
        Debug.Log("Game Paused");
    }
    
    public void ResumeGame()
    {
        Debug.Log("ResumeGame function called");
        
        isPaused = false;
        Time.timeScale = 1f; // Resume normal time
        
  
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        

        var playerInput = FindObjectOfType<PlayerInputHandler>();
        if (playerInput != null)
        {
            playerInput.enabled = true;
        }
        
        Debug.Log("Game Resumed");
    }
    
    public void QuitToMainMenu()
    {
        Debug.Log("QuitToMainMenu function called");
        

        isPaused = false;
        Time.timeScale = 1f;
        

        if (GameManager.Instance != null)
        {
            Debug.Log("Using GameManager to quit to main menu");
            GameManager.Instance.QuitToMainMenu();
        }
        else
        {

            Debug.LogWarning("GameManager.Instance is null! Using direct scene loading.");
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
    
    public bool IsPaused()
    {
        return isPaused;
    }
}