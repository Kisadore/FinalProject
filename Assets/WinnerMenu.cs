using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinnerMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject winnerPanel;
    public TextMeshProUGUI winnerText;
    public TextMeshProUGUI statsText;
    public Button restartButton;
    public Button quitButton;
    
    [Header("Effects")]
    public AudioClip victorySound;
    
    private AudioSource audioSource;
    
    void Start()
    {
        // Hide the winner panel initially
        if (winnerPanel != null)
        {
            winnerPanel.SetActive(false);
        }
        
        // Set up audio
        audioSource = gameObject.AddComponent<AudioSource>();
        
        // Set up button listeners
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartClicked);
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitClicked);
        }
    }
    
    public void ShowWinnerScreen()
    {
        Debug.Log("Showing winner screen");
        
        if (winnerPanel != null)
        {
            winnerPanel.SetActive(true);
        }
        
        // Play victory sound
        if (victorySound != null && audioSource != null)
        {
            audioSource.PlayOneShot(victorySound);
        }
        
        // Update winner text
        if (winnerText != null)
        {
            winnerText.text = "Congratulations!\nYou found all the coins!";
        }
        
        // Show stats
        if (statsText != null && GameManager.Instance != null)
        {
            float timeRemaining = GameManager.Instance.GetTimeRemaining();
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            
            statsText.text = $"Time Remaining: {minutes:00}:{seconds:00}\n" +
                           $"Difficulty: {GameManager.Instance.currentDifficulty}";
        }
        
        // Show cursor for menu interaction
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        // Disable player controls
        var playerInput = FindObjectOfType<PlayerInputHandler>();
        if (playerInput != null)
        {
            playerInput.enabled = false;
        }
    }
    
    void OnRestartClicked()
    {
        Debug.Log("Restart button clicked");
        
        if (winnerPanel != null)
        {
            winnerPanel.SetActive(false);
        }
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.RestartGame();
        }
    }
    
    void OnQuitClicked()
    {
        Debug.Log("Quit button clicked");
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.QuitToMainMenu();
        }
    }
}