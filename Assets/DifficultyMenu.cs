using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyMenu : MonoBehaviour
{
    [Header("UI References")]
    public Button easyButton;
    public Button hardButton;
    public Button backButton;
    
    [Header("Difficulty Descriptions")]
    public TextMeshProUGUI easyDescription;
    public TextMeshProUGUI hardDescription;
    
    private Canvas canvas;
    
    void Start()
    {
        canvas = GetComponent<Canvas>();
        
        // Set up button listeners
        easyButton.onClick.AddListener(() => StartGame(GameManager.GameDifficulty.Easy));
        hardButton.onClick.AddListener(() => StartGame(GameManager.GameDifficulty.Hard));
        backButton.onClick.AddListener(CloseDifficultyWindow);
        
        // Set description texts
        if (easyDescription != null)
        {
            easyDescription.text = "Start on the ship.\nFind coins before time runs out.\nNo oxygen management.";
        }
        
        if (hardDescription != null)
        {
            hardDescription.text = "Start underwater.\nManage your oxygen supply.\nSurface periodically to breathe.";
        }
    }
    
    public void OpenDifficultyWindow()
    {
        canvas.enabled = true;
    }
    
    public void CloseDifficultyWindow()
    {
        canvas.enabled = false;
    }
    
    void StartGame(GameManager.GameDifficulty difficulty)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame(difficulty);
        }
        else
        {
            Debug.LogError("GameManager not found! Make sure it exists in the scene.");
        }
    }
}