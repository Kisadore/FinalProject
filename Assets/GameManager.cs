using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public enum GameDifficulty
    {
        Easy,
        Hard
    }
    
    public GameDifficulty currentDifficulty = GameDifficulty.Easy;
    public float gameTime = 300f; // 5 minutes
    public float oxygenTime = 60f; // 1 minute of oxygen
    public int coinsRequired = 10;
    public int coinsCollected = 0;
    
    private float currentTime;
    private float currentOxygen;
    private bool isGameActive = false;
    private bool hasSlowedDown = false;
    
    // Events for game state changes
    public UnityEvent OnGameOver = new UnityEvent();
    public UnityEvent<string> OnGameOverWithReason = new UnityEvent<string>();
    
    void Awake()
    {
        Debug.Log($"GameManager Awake - Current difficulty: {currentDifficulty}");
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("GameManager instance created and marked as DontDestroyOnLoad");
        }
        else
        {
            Debug.Log("Duplicate GameManager found, destroying...");
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        Debug.Log($"GameManager Start - Current difficulty: {currentDifficulty}");
    }
    
    public void StartGame(GameDifficulty difficulty)
    {
        Debug.Log($"StartGame called with difficulty: {difficulty}");
        
        currentDifficulty = difficulty;
        currentTime = gameTime;
        
        // Initialize oxygen properly based on difficulty
        if (difficulty == GameDifficulty.Hard)
        {
            currentOxygen = oxygenTime;
        }
        else
        {
            currentOxygen = float.MaxValue; // Set to max in Easy mode to prevent false triggers
        }
        
        coinsCollected = 0;
        isGameActive = true;
        hasSlowedDown = false;
        Time.timeScale = 1f; // Reset time scale
        
        // Store the difficulty for backup
        PlayerPrefs.SetInt("GameDifficulty", (int)difficulty);
        PlayerPrefs.Save();
        
        Debug.Log($"Difficulty set to: {currentDifficulty}");
        Debug.Log($"PlayerPrefs saved with difficulty: {(int)difficulty}");
        
        // Small delay to ensure everything is set
        StartCoroutine(LoadGameSceneWithDelay());
    }
    
    System.Collections.IEnumerator LoadGameSceneWithDelay()
    {
        Debug.Log($"About to load game scene, current difficulty: {currentDifficulty}");
        yield return new WaitForSeconds(0.1f);
        
        Debug.Log($"Loading game scene now, difficulty is: {currentDifficulty}");
        SceneManager.LoadScene("GameScene"); // Replace with your actual game scene name
    }
    
    void Update()
    {
        if (isGameActive)
        {
            currentTime -= Time.deltaTime;
            
            // Check for time running out
            if (currentTime <= 0)
            {
                GameOver("Time's up!");
                return;
            }
            
            // Check for critical time (1 minute remaining)
            bool isCriticalTime = currentTime <= 60f;
            
            // Only check oxygen in Hard mode
            bool isCriticalOxygen = false;
            if (currentDifficulty == GameDifficulty.Hard)
            {
                isCriticalOxygen = currentOxygen <= 60f;
            }
            
            // Slow time when critical (either time or oxygen is low)
            if ((isCriticalTime || isCriticalOxygen) && !hasSlowedDown)
            {
                Time.timeScale = 0.5f;
                hasSlowedDown = true;
                Debug.Log($"Critical state - Time: {isCriticalTime}, Oxygen: {isCriticalOxygen}");
            }
        }
    }
    
    public void UpdateOxygen(float deltaTime)
    {
        if (currentDifficulty == GameDifficulty.Hard && isGameActive)
        {
            currentOxygen -= deltaTime;
            
            if (currentOxygen <= 0)
            {
                GameOver("Out of oxygen!");
            }
        }
    }
    
    public void RefillOxygen()
    {
        currentOxygen = oxygenTime;
    }
    
    public void CollectCoin()
    {
        coinsCollected++;
        Debug.Log($"Coin collected! Total: {coinsCollected}/{coinsRequired}");
        
        if (coinsCollected >= coinsRequired)
        {
            Victory();
        }
    }

    void Victory()
    {
        isGameActive = false;
        Time.timeScale = 1f;
        Debug.Log("Victory! All coins collected!");
        
        // Show winner screen
        var winnerMenu = FindObjectOfType<WinnerMenu>();
        if (winnerMenu != null)
        {
            winnerMenu.ShowWinnerScreen();
        }
    }
    
    public void AddTime(float bonusTime)
    {
        currentTime += bonusTime;
    }
    
    public void GameOver(string reason = "")
    {
        isGameActive = false;
        Time.timeScale = 1f;
        Debug.Log($"Game Over! Reason: {reason}");
        
        // Trigger game over events
        OnGameOver?.Invoke();
        OnGameOverWithReason?.Invoke(reason);
        
        // Disable player controls (optional)
        DisablePlayerControls();
    }
    
    void DisablePlayerControls()
    {
        // Find and disable player input
        var playerInput = FindObjectOfType<PlayerInputHandler>();
        if (playerInput != null)
        {
            playerInput.enabled = false;
        }
        
        // Show cursor for menu interaction
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void RestartGame()
    {
        Debug.Log("Restarting game...");
        Time.timeScale = 1f;
        
        // Keep the same difficulty
        StartGame(currentDifficulty);
    }
    
    public void QuitToMainMenu()
    {
        Debug.Log("Quitting to main menu...");
        Time.timeScale = 1f;
        Destroy(gameObject); // Destroy the GameManager so a fresh one can be created
        SceneManager.LoadScene("MainMenu"); // Replace with your main menu scene name
    }
    
    void NextLevel()
    {
        isGameActive = false;
        Time.timeScale = 1f;
        Debug.Log("Level Complete!");
        // TODO: Load next level
    }
    
    public float GetTimeRemaining() => currentTime;
    public float GetOxygenRemaining() => currentOxygen;
    public float GetOxygenPercentage() => currentOxygen / oxygenTime;
    public bool IsGameActive() => isGameActive;
}