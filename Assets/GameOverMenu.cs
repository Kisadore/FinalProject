using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverReasonText;
    public Button restartButton;
    public Button quitButton;
    
    [Header("Audio")]
    public AudioClip gameOverSound;
    [Range(0f, 1f)]
    public float volume = 1.0f;
    
    private AudioSource audioSource;
    
    void Start()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(OnRestartClicked);
        }
        
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(OnQuitClicked);
        }
        

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null && gameOverSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
        

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOverWithReason.AddListener(ShowGameOver);
        }
    }
    
    void OnDestroy()
    {

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnGameOverWithReason.RemoveListener(ShowGameOver);
        }
    }
    
    public void ShowGameOver(string reason)
    {
        Debug.Log($"Showing game over menu. Reason: {reason}");
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        
        if (gameOverReasonText != null)
        {
            gameOverReasonText.text = $"Game Over!\n{reason}";
        }
        

        if (gameOverSound != null)
        {
            if (audioSource != null)
            {
                audioSource.clip = gameOverSound;
                audioSource.volume = volume;
                audioSource.Play();
            }
            else
            {

                AudioSource.PlayClipAtPoint(gameOverSound, Camera.main.transform.position, volume);
            }
            
            Debug.Log("Playing game over sound");
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    void OnRestartClicked()
    {
        Debug.Log("Restart button clicked");
        
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
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